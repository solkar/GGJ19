using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshSearchTest : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    public NavMeshAgent enemyAgent;

    [SerializeField]
    public Transform target;

    #endregion

    IEnumerator Start()
    {
        var wanderingState = new Enemies.EnemyNavMeshSearch.State(
            enemyAgent,
            target
        );

        StartCoroutine(wanderingState.Enter());

        yield break;
    }
}
