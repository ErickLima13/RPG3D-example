using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ItemBeginDragged; 
    Vector3 startPos;
    Transform startParent;

    public Item item;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = item.icon;
    }

    //começa a arrastar o objeto na cena
    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemBeginDragged = gameObject;
        startPos = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }


    //quando estiver arrastando o objeto na cena
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    //quando para de arrastar o objeto na cena
    public void OnEndDrag(PointerEventData eventData)
    {
        ItemBeginDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == startParent)
        {
            transform.position = startPos;
        }
    }

    public void SetParent(Transform slotTransform, Slots slot)
    {

        if (item.SlotType.ToString() ==  slot.SlotType.ToString())
        {
            transform.SetParent(slotTransform);
            item.GetAction();
            

        }
        else if (slot.SlotType.ToString() == "Inventory")
        {
            transform.SetParent(slotTransform);
            item.RemoveStats();
        }
    }
    

    
}
