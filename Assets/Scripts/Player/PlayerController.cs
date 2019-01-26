using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 6.0f, jumpSpeed = 8.0f, dashLength = 2.0f, dashRefillRate = 5.0f;
    [SerializeField]
    int numberOfDashesAvailable = 1;

    enum PlayerState
    {
        //Things that depend on inputs/controllers
        idle, walking, attacking, dashing
    }

    PlayerState currentState;
    private Vector3 lastPosition = new Vector3(0, 0, 0);
    private float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //gameObject.transform.position = new Vector3(0, 5, 0);
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