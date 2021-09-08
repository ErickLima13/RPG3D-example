using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public float Speed;
    public float RotSpeed;
    private float Rotation;
    public float Gravity;

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
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GetMouseInput();
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
                    StartCoroutine(Attack(1));
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
                    StartCoroutine(Attack(0));

                }
            }
        }
    }

    IEnumerator Attack(int transitionValue)
    {
        if (!isReady)
        {
            isReady = true;
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);
            yield return new WaitForSeconds(1.3f);

            GetEnemiesRange();
            foreach (Transform enemies in EnemiesList)
            {
                //executar ação de dano no inimigo
                Enemy enemy = enemies.GetComponent<Enemy>();

                if(enemy != null)
                {
                    enemy.GetHit();
                }
            }

            anim.SetInteger("transition", transitionValue);
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
}
