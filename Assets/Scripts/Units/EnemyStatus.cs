using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Enemies
{
    public class EnemyStatus : MonoBehaviour
    {
        public TextMeshProUGUI text;

        IEnumerable Start()
        {
            Enemy enemy = null;
            while ((enemy = GetComponentInParent<Enemy>()) == null)
            {
                Debug.Log("enemy is... null");

                yield return null;
            }

            while (true)
            {
                Debug.Log("status " + enemy.state);

                switch (enemy.state)
                {
                    case Enemy.State.Idle:
                        text.text = string.Empty;
                        break;

                    case Enemy.State.Wandering:
                        text.text = "?";
                        break;

                    case Enemy.State.FollowHero:
                        text.text = "!";
                        break;
                }

                yield return null;
            }
        }
    }
}