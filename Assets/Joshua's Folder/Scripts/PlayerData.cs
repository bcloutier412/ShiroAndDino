using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    
    public float doubleHitWindow = 2.0f; // Time allowed for double hit
    public float doubleHitCooldown = 2.0f; // Cooldown duration
    public float invincibilityDuration = 1.5f;  
    
    public int maxHealth = 10;  
}
