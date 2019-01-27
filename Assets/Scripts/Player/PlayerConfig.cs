
using UnityEngine;

[System.Serializable()]
public struct PlayerScriptableParameters
{
    [Header("Character Movement")]
    public float speed;
    public float dashLength;
    public float dashDuration;
    public float dashRefillRate;
    public int numberOfDashesAvailable;

    [Header("Character Management")]
    public int characterHealth;
    public float hitStunTime; 
    public float invincibilityTime;

    [Header("Character Combat System")]
    public int attackDamage;
    public float attackRange;
    public float attackWindUpTime;
    public float attackStrikeTime;
    public float attackRecoverTime;
    public float attackKnockBack;

    [Header("Character Animation System")]
    public AnimationClip attackAnimation;
}

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public PlayerScriptableParameters playerConfig;
}

