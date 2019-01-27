using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParametersManager : MonoBehaviour
{
    [SerializeField]
    int characterHealth, dashingPoints;
    [SerializeField]
    float characterHitStunTime, characterInvincibilityTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void PlayerTookHit(int damage)
    {
        characterHealth -= damage;
    }

    private void PlayerGotInvincible()
    {

    }

    IEnumerator RefillDashingPoints(float time)
    {
        yield return new WaitForSeconds(time);
        dashingPoints++;
    }
}
