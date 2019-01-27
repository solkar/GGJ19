using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerConfig parameters;

    private float speed, dashLength, dashRefillRate, attackHitPoints, attackRange, initialSpeed;
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
            return stateMachine.GetCurrentState() == CharacterStateMachine.CharacterState.idle ||
                    stateMachine.GetCurrentState() == CharacterStateMachine.CharacterState.walking ||
                    stateMachine.GetCurrentState() == CharacterStateMachine.CharacterState.dashing;
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
        //Dash stuff
        dashRefillRate = parameters.playerConfig.dashRefillRate;
        dashLength = parameters.playerConfig.dashLength;
        numberOfDashesAvailable = parameters.playerConfig.numberOfDashesAvailable;
        maxDashAvailable = parameters.playerConfig.numberOfDashesAvailable;
        initialSpeed = speed;
    }

    void Update()
    {
        //If Fire1 and not attacking, the player can attack
        if (Input.GetAxisRaw("Fire1") != 0)
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

        if(Input.GetAxisRaw("Fire1") == 0) // reset attack input gate
        {
            isAttackAxisAlreadyDown = false;
        }

        //Ff Fire2 and there are dashing points available, the player will dash
        if (Input.GetAxisRaw("Fire2") != 0)
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

        if(Input.GetAxisRaw("Fire2") == 0) // reset dash input gate
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
        yield return new WaitForSeconds(parameters.playerConfig.attackAnimation.length);

        stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.idle);
    }

    //Hitting the enemy
    public static bool HitCheck(Transform target, Transform origin, float distance, float range)
    {
        var vectorToCollider = (target.position - origin.position);
        if (vectorToCollider.sqrMagnitude < distance * distance)
        {
            var angle = Mathf.Cos(range / 2 * Mathf.Deg2Rad);
            if (Vector3.Dot(vectorToCollider.normalized, origin.forward) > angle)
            {
                return true;
            }
        }
        return false;
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
}