using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderingTest : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    public NavMeshAgent enemyAgent;

    [SerializeField]
    public Enemies.EnemyWandering settings;

    #endregion

    IEnumerator Start()
    {
        var wanderingState = new Enemies.EnemyWandering.State(
            settings.settings,
            enemyAgent
        );
        
        StartCoroutine(wanderingState.Enter());

        yield break;
    }
}