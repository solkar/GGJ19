using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public enum CharacterState
    {
        idle, walking, attacking, dashing, takingHit, dead
    }

    [SerializeField]
    private CharacterState currentState;

    [SerializeField]
    private bool displayDebug = false;

    private Animator playerAnimator;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        currentState = CharacterState.idle;
    }

    public CharacterState GetCurrentState()
    {
        return currentState;
    }

    public void RequestChangePlayerState(CharacterState? stateModifier)
    {
        if (stateModifier.HasValue && currentState != stateModifier)
        {
            currentState = stateModifier.Value;

            switch (currentState)
            {
                case CharacterState.idle:
                    IdleState();
                    break;
                case CharacterState.walking:
                    WalkingState();
                    break;
                case CharacterState.dashing:
                    DashingState();
                    break;
                case CharacterState.attacking:
                    AttackingState();
                    break;
                case CharacterState.takingHit:
                    TakingHitState();
                    break;
                case CharacterState.dead:
                    DeadState();
                    break;
            }

        }

        if(displayDebug)
            Debug.Log("Current state is " + currentState);
    }

    private void IdleState()
    {
        playerAnimator.SetBool("Idle", true);
    }

    private void WalkingState()
    {
        playerAnimator.SetBool("Idle", false);
        playerAnimator.SetTrigger("Walking");
    }

    private void TookHitState()
    {
        playerAnimator.SetTrigger("TookHit");
    }

    private void AttackingState()
    {
        playerAnimator.SetTrigger("Attacking");
    }

    private void DashingState()
    {
        playerAnimator.SetTrigger("Dashing");
    }

    private void TakingHitState()
    {
        playerAnimator.SetTrigger("TookHit");
    }

    private void DeadState()
    {
        playerAnimator.SetBool("Dead", true);
    }

}
