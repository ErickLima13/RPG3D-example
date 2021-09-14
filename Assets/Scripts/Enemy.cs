using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public float TotalHealth = 100f;
    public float CurrentHealth;
    public float AttackDamage;
    public float MovementSpeed;
    public float ColliderRadius;
    public float lookRadius = 10f;
    


    private Animator anim;
    private CapsuleCollider cap;
    public Transform target;
    private NavMeshAgent agent;

    bool isReady;
    public bool PlayerIsAlive;


    private void Start()
    {
        anim = GetComponent<Animator>();
        cap = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        CurrentHealth = TotalHealth;
        PlayerIsAlive = true;
    }

    private void Update()
    {
        if (CurrentHealth > 0)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                //o personagem está dentro do raio de ação
                //o inimigo deve seguir o personagem
                if (!anim.GetBool("attacking"))
                {
                    agent.isStopped = false;
                    agent.SetDestination(target.position);
                    anim.SetInteger("transition", 2);
                    anim.SetBool("walking", true);
                }

                if (distance <= agent.stoppingDistance)
                {
                    //aqui o personagem está dentro do raio de ataque
                    //ação de atacar
                    StartCoroutine("Attack");
                    LookTarget();
                }

                if (distance >= agent.stoppingDistance)
                {
                    anim.SetBool("attacking", false);
                }

            }
            else
            {
                // é chamado quando o personagem está completamente fora do raio de ação
                anim.SetInteger("transition", 0);
                anim.SetBool("attacking", false);
                anim.SetBool("walking", false);
                agent.isStopped = true;
            }
        }
    }

    IEnumerator Attack()
    {
        if (!isReady && PlayerIsAlive && !anim.GetBool("hiting"))
        {
            isReady = true;
            anim.SetBool("attacking", true);
            anim.SetBool("walking", false);
            anim.SetInteger("transition", 1);
            yield return new WaitForSeconds(1f);
            GetEnemy();
            yield return new WaitForSeconds(1.7f);
            isReady = false;

        }
        if(!PlayerIsAlive)
        {
            anim.SetInteger("transition", 0);
            anim.SetBool("walking", false);
            anim.SetBool("attacking", false);
            agent.isStopped = true;
        }
    }

    void GetEnemy()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
        {
            if (c.gameObject.CompareTag("Player"))
            {
                //print("atacou o player");
                //detecta o player
                c.gameObject.GetComponent<Player>().GetHit(25f);
                PlayerIsAlive = c.gameObject.GetComponent<Player>().isAlive ;

            }
        }
    }

    void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position + transform.forward, ColliderRadius);
    }

    public void GetHit(float Damage)
    {
        CurrentHealth -= Damage;

        if (CurrentHealth > 0)
        {
            //print("inimigo tomou dano");
            StopCoroutine("Attack");
            anim.SetInteger("transition", 3);
            anim.SetBool("hiting", true);
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
        yield return new WaitForSeconds(1f);
        anim.SetInteger("transition", 0);
        anim.SetBool("hiting", false);
        isReady = false;

    }

    

}
