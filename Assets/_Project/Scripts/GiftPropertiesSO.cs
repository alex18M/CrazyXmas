using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GiftPropertiesSO : ScriptableObject
{
    public string type;
    public float health;
    public float maxHealth;
    public int points;
    public bool IsGift;
}
