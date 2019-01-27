using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerConfig parameters;

    private float speed, dashLength, dashRefillRate, attackHitPoints, attackRange, initialSpeed;
    private int numberOfDashesAvailable;

    private readonly float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero, lastPosition = new Vector3(0, 0, 0);
    private CharacterController controller;
    private CharacterStateMachine stateMachine;
    private bool playerCanMove = true;
    Animator animator;
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
        initialSpeed = speed;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Axe_Attack"))
            playerCanMove = false;
        else
            playerCanMove = true;

        //If can move and its not attacking, change the state to walking
        if (playerCanMove)
        {
            MovePlayer();
            if (lastPosition != gameObject.transform.position)
                stateMachine.RequestChangePlayerState(stateModifier: CharacterStateMachine.CharacterState.walking);
            else
                stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.idle);
        }

        //If Fire1 and not attacking, the player can attack
        if (Input.GetAxisRaw("Fire1") != 0 && stateMachine.GetCurrentState() != CharacterStateMachine.CharacterState.attacking)
        {
            stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.attacking);
            PerformAttack();
        }
        //Ff Fire2 and there are dashing points available, the player will dash
        if (Input.GetAxisRaw("Fire2") != 0 && numberOfDashesAvailable > 0 && stateMachine.GetCurrentState() == CharacterStateMachine.CharacterState.walking)
        {
            stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.dashing);
            PerformeDashing();
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

    private void PerformeDashing()
    {
        StartCoroutine(ExecuteDash(0.6f));
        numberOfDashesAvailable--;
        stateMachine.RequestChangePlayerState(stateModifier: CharacterStateMachine.CharacterState.walking);
    }

    private void PerformAttack()
    {
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
        speed *= dashLength;
        yield return new WaitForSeconds(interval);
        speed = initialSpeed;
    }
}