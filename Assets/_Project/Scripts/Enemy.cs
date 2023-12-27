using System;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public EnemyPropertiesSO enemyProperties;

    private void Start()
    {
        if (enemyProperties != null)
        {
            // Configurar el enemigo con las propiedades del ScriptableObject
            ConfigureEnemy();
        }
        else
        {
            Debug.LogError("EnemyProperties not assigned to enemy: " + name);
        }
    }

    private void ConfigureEnemy()
    {
       
        SetHealth(enemyProperties.health);
        SetAttackDamage(enemyProperties.attackDamage);
        SetMovementSpeed(enemyProperties.movementSpeed);
    }

    private void SetHealth(float value)
    {
        // Implementa lógica para configurar la salud del enemigo
        enemyProperties.health = enemyProperties.enemyType switch
        {
            "Elf" => enemyProperties.maxHealth,
            "SnowMan" => enemyProperties.maxHealth,
            "GingerCookie" => enemyProperties.maxHealth,
            _ => enemyProperties.maxHealth
        };
    }

    private void SetAttackDamage(float value)
    {
        enemyProperties.health = enemyProperties.enemyType switch
        {
            "Elf" => enemyProperties.attackDamage,
            "SnowMan" => enemyProperties.attackDamage,
            "GingerCookie" => enemyProperties.attackDamage,
            _ => enemyProperties.attackDamage
        };
    }

    private void SetMovementSpeed(float value)
    {
        enemyProperties.health = enemyProperties.enemyType switch
        {
            "Elf" => enemyProperties.movementSpeed,
            "SnowMan" => enemyProperties.movementSpeed,
            "GingerCookie" => enemyProperties.movementSpeed,
            _ => enemyProperties.movementSpeed
        };
    }
}