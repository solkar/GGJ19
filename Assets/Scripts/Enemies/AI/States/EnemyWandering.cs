using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyWandering", menuName = "Scriptable Objects/EnemyWanderingSettings")]
    public class EnemyWandering : ScriptableObject
    {
        [Serializable]
        public class Settings
        {
            [SerializeField]
            public Vector3 aroundPosition;

            [SerializeField]
            public float wanderingDirectionChangePeriod = 1;

            [SerializeField]
            public float wanderingDistance = 1;

            [SerializeField]
            public float wanderingSpeed = 4;
        }

        [SerializeField]
        public Settings settings;

        public class State : IFSMState
        {
            private Settings settings;
            private NavMeshAgent agent;

            public State(
                Settings settings,
                NavMeshAgent agent)
            {
                this.settings = settings;
                this.agent = agent;
            }

            public IEnumerator Enter()
            {
                agent.destination = agent.transform.position;

                float lastTime = Time.time;

                while (true)
                {
                    // One it reaches the previous destination, search for another randome place wehre to go
                    if (Mathf.Abs(Time.time - lastTime) > settings.wanderingDirectionChangePeriod)
                    {
                        lastTime = Time.time;

                        var delta = UnityEngine.Random.rotation * Vector3.right * settings.wanderingDistance;

                        agent.speed = settings.wanderingSpeed;
                        agent.destination = settings.aroundPosition + delta;
                    }

                    yield return null;
                }
            }

            public void Exit()
            {
            }
        }
    }
}