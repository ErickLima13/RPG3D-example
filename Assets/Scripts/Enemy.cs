using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public float TotalHealth = 100f;
    public float CurrentHealth;
    public float AttackDamage;
    public float MovementSpeed;

    private Animator anim;
    private CapsuleCollider cap;

    private void Start()
    {
        anim = GetComponent<Animator>();
        cap = GetComponent<CapsuleCollider>();

        CurrentHealth = TotalHealth;
    }

    private void Update()
    {
        
    }

    public void GetHit(float Damage)
    {
        CurrentHealth -= Damage;

        if (CurrentHealth > 0)
        {
            print("Dano");
            anim.SetInteger("transition", 3);
            StartCoroutine(RecoveryFromHit());
        }
        else
        {
            anim.SetInteger("transition", 4);
            cap.enabled = false;
        }
    }
   
    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("transition", 0);

    }

    void Die()
    {
        if(CurrentHealth < 0)
        {
            anim.SetInteger("transition", 4);
            //Destroy(gameObject, 2f);
            print("morreu");
        }
    }

}
