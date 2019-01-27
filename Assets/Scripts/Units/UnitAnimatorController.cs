using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animator controller.
/// </summary>
[RequireComponent(typeof(Animator))]
public sealed class UnitAnimatorController : MonoBehaviour
{
    private Animator animator;

    protected void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    public void IdleState()
    {
        animator.SetBool("Idle", true);
    }

    public void WalkingState()
    {
        animator.SetBool("Idle", false);
        animator.SetTrigger("Walking");
    }

    public void TookHitState()
    {
        animator.SetTrigger("TookHit");
    }

    public void AttackingState()
    {
        animator.SetTrigger("Attacking");
    }

    public void DashingState()
    {
        animator.SetTrigger("Dashing");
    }

    public void TakingHitState()
    {
        animator.SetTrigger("TookHit");
    }

    public void DeadState()
    {
        animator.SetBool("Dead", true);
    }
}

