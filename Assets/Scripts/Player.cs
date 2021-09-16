using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public float Speed;
    public float RotSpeed;
    private float Rotation;
    public float Gravity;
    public float EnemyDamage = 25f;
    public float TotalHealth = 100f;
    public float CurrentHealth;
    public bool isAlive;

    Vector3 MoveDirection;

    CharacterController controller;
    Animator anim;
    

    bool isReady;

    List<Transform> EnemiesList = new List<Transform>();
    public float ColliderRadius;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        isAlive = true;
        CurrentHealth = TotalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            Move();
            GetMouseInput();
        }
       
        
    }

    private void Move()
    {
        if (controller.isGrounded)
        {          
            if (Input.GetKey(KeyCode.W))
            {
                if (!anim.GetBool("attacking"))
                {
                    anim.SetBool("walking", true);
                    anim.SetInteger("transition", 1);
                    MoveDirection = Vector3.forward * Speed;
                    MoveDirection = transform.TransformDirection(MoveDirection);
                }
                else
                {
                    anim.SetBool("walking", false);
                    MoveDirection = Vector3.zero;
                    //StartCoroutine(Attack(1));
                }                   
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("walking", false);
                anim.SetInteger("transition", 0);
                MoveDirection = Vector3.zero;
            }
        }

        Rotation += Input.GetAxis("Horizontal") * RotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, Rotation, 0);

        MoveDirection.y -= Gravity * Time.deltaTime;
        controller.Move(MoveDirection * Time.deltaTime);
       
    }

    void GetMouseInput()
    {
        if (controller.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (anim.GetBool("walking"))
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                }

                if (!anim.GetBool("walking"))
                {
                    //executar ataque
                    StartCoroutine("Attack");

                }
            }
        }
    }

    IEnumerator Attack()
    {
        if (!isReady && !anim.GetBool("hiting"))
        {
            isReady = true;
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);

            yield return new WaitForSeconds(0.5f);
            GetEnemiesRange();
            foreach (Transform enemies in EnemiesList)
            {
                //executar ação de dano no inimigo
                Enemy enemy = enemies.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.GetHit(EnemyDamage);
                    AudioController.current.PlayMusic(AudioController.current.sfxAxeHit);
                }
            }

            yield return new WaitForSeconds(0.8f);

            anim.SetInteger("transition", 0);
            anim.SetBool("attacking", false);
            isReady = false;
            
        }
    }
   
    void GetEnemiesRange()
    {
        EnemiesList.Clear();
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                EnemiesList.Add(c.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, ColliderRadius);
    }

    public void GetHit(float Damage)
    {
        
        CurrentHealth -= Damage;

        if (CurrentHealth > 0)
        {

            //print("player tomou dano");
            //toma hit aqui
            StopCoroutine("Attack");
            anim.SetInteger("transition", 3);
            anim.SetBool("hiting", true);
            StartCoroutine(RecoveryFromHit());
            AudioController.current.PlayMusic(AudioController.current.SfxDamagePlayer);

        }
        else
        {
            //morre aqui
            anim.SetInteger("transition", 4);
            isAlive = false;
            Speed = 0;
            RotSpeed = 0;



        }
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(1.1f);
        anim.SetInteger("transition", 0);
        anim.SetBool("hiting", false);
        isReady = false;
        anim.SetBool("attacking", false);
    }

    public void IncreaseStats(float Health, float IncreaseSpeed)
    {
        CurrentHealth += Health;
        Speed += IncreaseSpeed;
    }

    public void DecreaseStats(float Health, float IncreaseSpeed)
    {
        CurrentHealth -= Health;
        Speed -= IncreaseSpeed;
    }


}
