using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator anim;
    private BoxCollider box;

    public float ColliderRadius;
    public bool IsOpened;

    public List<Item> Items = new List<Item>();


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        GetPlayer();
    }

    void GetPlayer()
    {
        if (!IsOpened) 
        {
            foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    //if (Input.GetMouseButtonDown(0))
                    //Báu abre somente com o contato do player
                    OpenChest();
                }
            }
        }
    }

    void OpenChest()
    {
        foreach(Item i in Items)
        {
            i.GetAction();
        }
        anim.SetTrigger("open");
        IsOpened = true;
        box.enabled = false;
        AudioController.current.PlayMusic(AudioController.current.SfxOpenChest);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ColliderRadius);
    }
}
