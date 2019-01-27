using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AI;

namespace Enemies
{
    [CreateAssetMenu(fileName = "NavMeshSearch", menuName = "Scriptable Objects/EnemyAttack")]
    public class EnemyAttack : ScriptableObject
    {
        [Serializable]
        public struct Settings
        {
            public float detectDistance;
            public float attackDistance;
            public float attackSpeed;
            public float attackRange;
            public float timeBetweenAttacks;
            public float timeDuration;
        }

        [SerializeField]
        public Settings settings;

        public class State : IFSMState
        {
            enum SubState
            {
                GettingClose,
                Attack,
                Waitting,
            }

            public bool done { get; private set; }

            private CharacterStateMachine animatorFSM;
            private Settings settings;
            private Transform target;
            private NavMeshAgent agent;

            public State(
                CharacterStateMachine animatorFSM,
                Settings settings,
                NavMeshAgent agent,
                Transform target
            )
            {
                this.animatorFSM = animatorFSM;
                this.settings = settings;
                this.target = target;
                this.agent = agent;
            }

            public IEnumerator Enter()
            {
                done = false;

                animatorFSM.RequestChangePlayerState(CharacterStateMachine.CharacterState.attacking);

                yield return new WaitForSeconds(settings.timeBetweenAttacks);

                if (Hit.HitCheck(target, agent.transform, settings.attackDistance, settings.attackRange))
                {
                    target.GetComponent<PlayerController>()?.TakeDamage(1);
                    Debug.Log("Damageeee!!!");
                }

                done = true;
            }

            public void OnDrawGizmos()
            {
#if UNITY_EDITOR
                var transform = agent.transform;
                var originalColor = UnityEditor.Handles.color;
                var range = settings.attackRange;
                var attackDistante = settings.attackDistance;

                UnityEditor.Handles.color = new Color(1, 0, 0, .1f);

                UnityEditor.Handles.DrawSolidArc(
                    transform.position,
                    transform.up,
                    Quaternion.Euler(0, -range / 2, 0) * transform.forward,
                    range,
                    attackDistante);

                UnityEditor.Handles.color = new Color(0, 1, 0, .1f);

                UnityEditor.Handles.DrawSolidArc(
                    transform.position,
                    transform.up,
                    Vector3.forward,
                    360,
                    settings.detectDistance);

                UnityEditor.Handles.color = originalColor;
#endif
            }

            public void Exit()
            {
            }
        }
    }
}