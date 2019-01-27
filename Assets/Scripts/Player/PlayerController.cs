using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerConfig parameters;

    private float speed, dashLength, dashRefillRate, attackHitPoints, attackAngle, attackRange, initialSpeed;
    public int numberOfDashesAvailable;
    private int maxDashAvailable;

    private readonly float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero, lastPosition = new Vector3(0, 0, 0);
    private CharacterController controller;
    private CharacterStateMachine stateMachine;
    Animator animator;

    private bool isAttackAxisAlreadyDown = false;
    private bool isDashAxisAlreadyDown = false;

    private bool canAttack
    {
        get
        {
            return stateMachine.GetCurrentState() == CharacterStateMachine.CharacterState.idle ||
                    stateMachine.GetCurrentState() == CharacterStateMachine.CharacterState.walking;
        }
    }

    private bool canDash
    {
        get
        {
            return numberOfDashesAvailable > 0 &&
                    (stateMachine.GetCurrentState() == CharacterStateMachine.CharacterState.idle ||
                    stateMachine.GetCurrentState() == CharacterStateMachine.CharacterState.walking);
        }
    }

    private bool canMove
    {
        get
        {
            var currentState = stateMachine.GetCurrentState();

            return
                currentState == CharacterStateMachine.CharacterState.takingHit ||
                currentState == CharacterStateMachine.CharacterState.idle ||
                currentState == CharacterStateMachine.CharacterState.walking ||
                currentState == CharacterStateMachine.CharacterState.dashing;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<CharacterStateMachine>();
        controller = GetComponent<CharacterController>();
        //Player setup stuff
        speed = parameters.playerConfig.speed;
        attackHitPoints = parameters.playerConfig.attackDamage;
        attackRange = parameters.playerConfig.attackRange;
        attackAngle = parameters.playerConfig.attackAngle;
        //Dash stuff
        dashRefillRate = parameters.playerConfig.dashRefillRate;
        dashLength = parameters.playerConfig.dashLength;
        numberOfDashesAvailable = parameters.playerConfig.numberOfDashesAvailable;
        maxDashAvailable = parameters.playerConfig.numberOfDashesAvailable;
        initialSpeed = speed;
    }

    void Update()
    {
        var fireButton1 = Input.GetAxisRaw("Fire1") != 0 || Input.GetKeyDown(KeyCode.Space);
        var fireButton2 = Input.GetAxisRaw("Fire2") != 0;

        //If Fire1 and not attacking, the player can attack
        if (fireButton1)
        {
            if(isAttackAxisAlreadyDown == false) // gate to only take attack input the frame the butotn is down
            {
                isAttackAxisAlreadyDown = true;
                if(canAttack)
                {
                    stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.attacking);
                    StartCoroutine(ReturnToIdleWhenAttackIsOver());
                }
            }
        }

        if (!fireButton1) // reset attack input gate
        {
            isAttackAxisAlreadyDown = false;
        }

        //Ff Fire2 and there are dashing points available, the player will dash
        if (fireButton2)
        {
            if (isDashAxisAlreadyDown == false) // gate to only take dash input the frame the button is down
            {
                isDashAxisAlreadyDown = true;
                if(canDash)
                {
                    stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.dashing);
                    PerformDashing();
                }
            }
        }

        if(!fireButton2) // reset dash input gate
        {
            isDashAxisAlreadyDown = false;
        }

        //If can move and its not attacking, change the state to walking
        if (canMove)
        {
            MovePlayer();

            if(stateMachine.GetCurrentState() != CharacterStateMachine.CharacterState.dashing)
            {
                if (lastPosition != gameObject.transform.position)
                    stateMachine.RequestChangePlayerState(stateModifier: CharacterStateMachine.CharacterState.walking);
                else
                    stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.idle);
            }
        }

        lastPosition = gameObject.transform.position;
    }

    private void MovePlayer()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized;
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);
        moveDirection = moveDirection * speed;
        // Apply movement based on gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void PerformDashing()
    {
        StartCoroutine(ExecuteDash(parameters.playerConfig.dashDuration));
    }

    private IEnumerator ReturnToIdleWhenAttackIsOver()
    {
        var delay = parameters.playerConfig.attackAnimation.length - parameters.playerConfig.attackAnimationDelay;

        yield return new WaitForSeconds(parameters.playerConfig.attackAnimationDelay);

        foreach (var enemy in Enemies.Enemy.list)
        {
            Debug.Log("One enemy found!");

            if (enemy != null && Hit.HitCheck(enemy.transform, transform, attackRange, attackAngle))
            {
                Debug.Log("And is being hit very hard!!!");

                enemy.GetComponent<UnitHealth>().TakeDamage(attackHitPoints);
            }
        }

        yield return new WaitForSeconds(delay);

        stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.idle);
    }

    IEnumerator ExecuteDash(float interval)
    {
        StopCoroutine(RefillDash());

        speed *= dashLength;
        yield return new WaitForSeconds(interval);
        speed = initialSpeed;
        
        stateMachine.RequestChangePlayerState(stateModifier: CharacterStateMachine.CharacterState.walking);

        numberOfDashesAvailable--;
        StartCoroutine(RefillDash());
    }

    public IEnumerator RefillDash()
    {
        yield return new WaitForSeconds(parameters.playerConfig.dashRefillRate);

        if(numberOfDashesAvailable < maxDashAvailable)
        {
            numberOfDashesAvailable++;
        }

        if (numberOfDashesAvailable < maxDashAvailable)
        {
            StartCoroutine(RefillDash());
        }
    }

    public void TakeDamage(float amount)
    {
        var health = GetComponent<UnitHealth>();
        if (health != null)
        {
            health.TakeDamage(amount);

            EventBus.OnPlayerDamage.Invoke();

            if (health.health <= 0)
            {
                stateMachine.RequestChangePlayerState(stateModifier: CharacterStateMachine.CharacterState.dead);
                enabled = false;

                EventBus.OnPlayerDead.Invoke();
            }
        }

        stateMachine.RequestChangePlayerState(stateModifier: CharacterStateMachine.CharacterState.takingHit);
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR

        var attackDistance = parameters.playerConfig.attackRange;
        var attackAngle = parameters.playerConfig.attackAngle;

        var originalColor = UnityEditor.Handles.color;

        UnityEditor.Handles.color = new Color(1, 0, 0, .1f);

        UnityEditor.Handles.DrawSolidArc(
            transform.position,
            transform.up,
            Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward,
            attackAngle,
            attackDistance);

        UnityEditor.Handles.color = originalColor;
#endif
    }
}