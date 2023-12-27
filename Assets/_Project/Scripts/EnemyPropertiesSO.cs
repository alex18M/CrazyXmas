using UnityEngine;

[CreateAssetMenu()]
public class EnemyPropertiesSO : ScriptableObject
{
    public string enemyType;
    public float health;
    public float maxHealth;
    public float attackDamage;
    public float attackCooldown;
    public float movementSpeed;
}