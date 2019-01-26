using UnityEngine;
using System.Collections;
using System;

public class GameStateFSM : MonoBehaviour
{
    public enum State
    {
        GameStarting,
        EnterHouse,
        ExitHouse,
        Ending,
        End,
    }

    #region serializable fields

    [SerializeField]
    private GameEnterHouseState.Settings enterHouse;

    [SerializeField]
    private GameExitHouseState.Settings exitHouse;

    #endregion

    [SerializeField]
    private State state;
    private float lastStateChange;
    private State stateRequest;
    private Coroutine lastCoroutine;

    protected void Awake()
    {
        //EventBus.OnEnterHouse.evt += () => { stateRequest = State.EnterHouse; };
        //EventBus.OnExitHouse.evt += () => { stateRequest = State.ExitHouse; };
    }

    IEnumerator Start()
    {
        // Initialize states of the state machine
        var enterHouseState = new GameEnterHouseState(enterHouse);
        var exitHouseState = new GameExitHouseState(exitHouse);

        yield return null;

        while (true)
        {
            var newState = state;
            try
            {
                // Evaluate conditions to change the behaviour
                {
                }

                if (newState != state)
                {
                    // Exit the previous state
                    if (lastCoroutine != null)
                    {
                        Debug.Log("Stopping previous state " + state);

                        StopCoroutine(lastCoroutine);

                        switch (state)
                        {
                            case State.EnterHouse:
                                enterHouseState.Exit();
                                break;

                            case State.ExitHouse:
                                enterHouseState.Exit();
                                break;
                        }
                    }

                    Debug.Log("Starting new state state " + newState);

                    // Enter the new state
                    switch (newState)
                    {
                        case State.EnterHouse:
                            {
                                lastCoroutine = StartCoroutine(enterHouseState.Enter());
                            }
                            break;

                        case State.ExitHouse:
                            {
                                lastCoroutine = StartCoroutine(exitHouseState.Enter());
                            }
                            break;

                    }
                    state = newState;
                    lastStateChange = Time.time;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Fuuuuuck!");
                Debug.LogException(e);
                newState = State.End;
            }

            yield return null;
        }
    }
}
