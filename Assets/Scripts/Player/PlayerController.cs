using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerConfig parameters;

    private float speed, dashLength, dashRefillRate, attackHitPoints, attackRange;
    private int numberOfDashesAvailable;

    private readonly float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero, lastPosition = new Vector3(0, 0, 0);
    private CharacterController controller;
    private CharacterStateMachine stateMachine;

    void Start()
    {
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

    }

    void Update()
    {
        if (stateMachine.GetCurrentState() != CharacterStateMachine.CharacterState.attacking)
            MovePlayer();

        if (lastPosition != gameObject.transform.position)
            stateMachine.RequestChangePlayerState(stateModifier: CharacterStateMachine.CharacterState.walking);
        else
            stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.idle);

        if (Input.GetKey(KeyCode.Mouse0) && stateMachine.GetCurrentState() != CharacterStateMachine.CharacterState.walking && stateMachine.GetCurrentState() != CharacterStateMachine.CharacterState.attacking)
        {
            stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.attacking);
            PerformAttack();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.dashing);
            PerformeDashing();
        }

        lastPosition = gameObject.transform.position;
        // Apply movement based on gravity

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void MovePlayer()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);
        moveDirection = moveDirection * speed;
        moveDirection.Normalize();
    }

    private void PerformeDashing()
    {
        moveDirection = moveDirection * dashLength;
        moveDirection.Normalize();
    }

    private void PerformAttack()
    {

    }

    //Hitting the enemy
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyManager>().TakeHit(amount: attackHitPoints);
            Debug.Log("Damageeeeee!!");
        }
        //var vectorToCollider = (collider.transform.position - transform.position);
        //if (vectorToCollider.sqrMagnitude < attackRange * attackRange)
        //{
        //var angle = Mathf.Cos(attackRange / 2 * Mathf.Deg2Rad);
        //if (Vector3.Dot(vectorToCollider.normalized, transform.forward) > angle)
        //{

        //}
        //}
    }

}