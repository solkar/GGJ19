using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void IdleState()
    {
        playerAnimator.SetBool("Idle", true);
    }

    void WalkingState()
    {
        playerAnimator.SetTrigger("Walking");
    }

    void TookHitState()
    {
        playerAnimator.SetTrigger("TookHit");
    }

    void AttackingState()
    {
        playerAnimator.SetTrigger("Attacking");
    }

    void DashingState()
    {
        playerAnimator.SetTrigger("Dashing");
    }

    void DeadState()
    {
        playerAnimator.SetBool("Dead", true);
    }
}
