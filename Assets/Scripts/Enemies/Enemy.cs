using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        public enum State
        {
            Idle,
            FollowHero,
            Attack,
            Wandering,
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

        #region private fields
        private State state = State.Idle;
        private Coroutine lastCoroutine;
        private float lastStateChange;
        private EnemyAttack.State attackState;
        #endregion

        IEnumerator Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            // Initialize states of the state machine
            var heroSearchState = new EnemyNavMeshSearch.State(
                heroSearch.settings,
                navMeshAgent,
                player.transform);

            // Initialize states of the state machine
            this.attackState = new EnemyAttack.State(
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
                                        newState = State.Wandering;
                                    }
                                }
                                break;
                        }

                        newState = State.FollowHero;
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

                        Debug.Log("Starting new state state " + newState);

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
