using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerConfig parameters;

    enum PlayerState
    {
        //Things that depend on inputs/controllers
        idle, walking, attacking, dashing
    }

    private float speed, dashLength, dashRefillRate, attackHitPoints, attackRange;
    private int numberOfDashesAvailable;

    private readonly float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero, lastPosition = new Vector3(0, 0, 0);
    private CharacterController controller;
    private PlayerState currentState;

    void Start()
    {
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
        if (currentState != PlayerState.attacking)
            MovePlayer();

        if (lastPosition != gameObject.transform.position)
        {
            currentState = PlayerState.walking;
            GetComponent<Animator>().SetBool("Idle", false);
            GetComponent<Animator>().SetTrigger("Walking");
        }
        else
        {
            currentState = PlayerState.idle;
            GetComponent<Animator>().SetBool("Idle", true);

        }

        if (Input.GetKey(KeyCode.Mouse0) && currentState != PlayerState.walking && currentState != PlayerState.attacking)
        {
            currentState = PlayerState.attacking;
            PerformAttack();
        }


        if (Input.GetKey(KeyCode.Mouse1))
        {
            currentState = PlayerState.dashing;
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
        GetComponent<Animator>().SetTrigger("Dashing");
        moveDirection = moveDirection * dashLength;
        moveDirection.Normalize();
    }

    private void PerformAttack()
    {
        GetComponent<Animator>().SetTrigger("Attacking");
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