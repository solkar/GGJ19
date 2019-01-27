using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(CharacterStateMachine))]
    public class Enemy : MonoBehaviour
    {
        public enum State
        {
            Idle,
            FollowHero,
            Attack,
            Wandering,
            Dead,
        }

        #region serialized fields

        [SerializeField]
        private NavMeshAgent navMeshAgent;

        [SerializeField]
        EnemyNavMeshSearch heroSearch;

        [SerializeField]
        private EnemyAttack enemyAttackSettings;

        [SerializeField]
        private EnemyWandering enemyWanderingSettings;

        #endregion

        public State state { get; private set; }

        #region private fields
        private Coroutine lastCoroutine;
        private float lastStateChange;
        private EnemyAttack.State attackState;
        #endregion

        public static List<Enemy> list = new List<Enemy>();

        private void Awake()
        {
            state = State.Idle;
            Enemy.list.Add(this);
        }

        private void OnDestroy()
        {
            Enemy.list.Remove(this);
        }

        IEnumerator Start()
        {
            var lastPosition = transform.position;

            var animatorFSM = GetComponent<CharacterStateMachine>();

            var health = GetComponent<UnitHealth>();

            var player = GameObject.FindGameObjectWithTag("Player");

            // Initialize states of the state machine
            var heroSearchState = new EnemyNavMeshSearch.State(
                heroSearch.settings,
                navMeshAgent,
                player.transform);

            // Initialize states of the state machine
            this.attackState = new EnemyAttack.State(
                animatorFSM,
                enemyAttackSettings.settings,
                navMeshAgent,
                player.transform);

            var wanderingState = new EnemyWandering.State(
                enemyWanderingSettings.settings,
                navMeshAgent);

            yield return null;

            // Initial positions
            //agent.destination = agent.transform.position;

            while (true)
            {
                var newState = state;
                try
                {
                    // Evaluate conditions to change the behaviour
                    {
                        switch (state)
                        {
                            case State.Idle:
                                {
                                    newState = State.Wandering;
                                }
                                break;

                            case State.Wandering:
                                {
                                    var distance = player.transform.position - transform.position;
                                    var d = enemyAttackSettings.settings.detectDistance;
                                    if (distance.sqrMagnitude < d * d)
                                    {
                                        newState = State.FollowHero;
                                    }
                                }
                                break;

                            case State.FollowHero:
                                {
                                    var distance = player.transform.position - transform.position;
                                    var d = enemyAttackSettings.settings.attackDistance;
                                    if (distance.sqrMagnitude < d * d)
                                    {
                                        newState = State.Attack;
                                    }

                                    var d2 = enemyAttackSettings.settings.detectDistance;

                                    if (distance.sqrMagnitude > d2 * d2)
                                    {
                                        newState = State.Wandering;
                                    }
                                }
                                break;

                            case State.Attack:
                                {
                                    if (attackState.done)
                                    {
                                        newState = State.Idle;
                                    }
                                }
                                break;
                        }

                        if (health != null && health.health <= 0)
                        {
                            newState = State.Dead;
                        }
                    }

                    // Always run
                    {
                        switch (newState)
                        {
                            case State.Idle:
                                {
                                    animatorFSM.RequestChangePlayerState(CharacterStateMachine.CharacterState.idle);
                                }
                                break;

                            case State.FollowHero:
                            case State.Wandering:
                                {
                                    if ((lastPosition - transform.position).sqrMagnitude < .1f)
                                    {
                                        animatorFSM.RequestChangePlayerState(CharacterStateMachine.CharacterState.idle);
                                    }
                                    else
                                    {
                                        animatorFSM.RequestChangePlayerState(CharacterStateMachine.CharacterState.walking);
                                    }
                                }
                                break;

                            case State.Attack:
                                {
                                    animatorFSM.RequestChangePlayerState(CharacterStateMachine.CharacterState.attacking);
                                }
                                break;

                            case State.Dead:
                                {
                                    animatorFSM.RequestChangePlayerState(CharacterStateMachine.CharacterState.dead);
                                }
                                break;
                        }

                        /*
                        // Ensure that the enemies are not on top of the enemies
                        var distance = (transform.position - player.transform.position).normalized;
                        if (distance.sqrMagnitude < 1)
                        {
                            transform.position = Vector3.Lerp(transform.position, player.transform.position + distance, 0.1f);
                        }
                        */
                    }


                    if (newState != state)
                    {
                        // Exit the previous state
                        if (lastCoroutine != null)
                        {
                            //Debug.Log("Stopping previous state " + state);

                            StopCoroutine(lastCoroutine);

                            switch (state)
                            {
                                case State.FollowHero:
                                    heroSearchState.Exit();
                                    break;

                                case State.Wandering:
                                    wanderingState.Exit();
                                    break;

                                case State.Attack:
                                    attackState.Exit();
                                    break;
                            }
                        }

                        //Debug.Log("Starting new state state " + newState);

                        // Enter the new state
                        switch (newState)
                        {
                            case State.FollowHero:
                                {
                                    lastCoroutine = StartCoroutine(heroSearchState.Enter());
                                }
                                break;

                            case State.Wandering:
                                {
                                    lastCoroutine = StartCoroutine(wanderingState.Enter());
                                }
                                break;

                            case State.Attack:
                                {
                                    lastCoroutine = StartCoroutine(attackState.Enter());
                                }
                                break;
                        }
                        state = newState;
                        lastStateChange = Time.time;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    newState = State.Idle;
                }

                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (this.attackState != null)
            {
                attackState.OnDrawGizmos();
            }
        }
    }
}
