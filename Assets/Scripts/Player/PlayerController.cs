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

    private float speed, dashLength, dashRefillRate, gravity = 20.0f;
    private int numberOfDashesAvailable = 1;
    private Vector3 moveDirection = Vector3.zero, lastPosition = new Vector3(0, 0, 0);
    private CharacterController controller;
    private PlayerState currentState;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = parameters.playerConfig.speed;
    }

    void Update()
    {
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

        lastPosition = gameObject.transform.position;
        // Apply movement based on gravity

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void MovePlayer()
    {

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        transform.rotation = Quaternion.LookRotation(moveDirection);
        moveDirection = moveDirection * speed;

    }

}