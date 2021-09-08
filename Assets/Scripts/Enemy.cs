using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public float TotalHealth;
    public float CurrentHealth;
    public float AttackDamage;
    public float MovementSpeed;

    public void GetHit()
    {
        print("Morri");
        Destroy(gameObject);
    }
   
}
