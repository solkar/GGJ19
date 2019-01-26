using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AI;

namespace Enemies
{
    [CreateAssetMenu(fileName = "NavMeshSearch", menuName = "Scriptable Objects/NavMeshSearch")]
    public class EnemyNavMeshSearch : ScriptableObject
    {
        [Serializable]
        public struct Settings
        {

        }

        [SerializeField]
        private Settings settings;

        public class State : IFSMState
        {
            private Transform target;
            private NavMeshAgent agent;

            public State(
                NavMeshAgent agent,
                Transform target
            )
            {
                this.target = target;
                this.agent = agent;
            }

            public IEnumerator Enter()
            {
                agent.destination = agent.transform.position;

                while (true)
                {
                    agent.destination = target.position;

                    while (agent.destination.XZ() != agent.transform.position.XZ())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(.5f);
                }
            }

            public void Exit()
            {
            }
        }
    }
}