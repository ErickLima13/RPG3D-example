using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slots : MonoBehaviour, IDropHandler
{
    [System.Serializable]
    public enum SlotsType
    {
        Inventory,
        Helmet,
        Shield,
        Armor,
        Trincket,
        Sword

    }

    public SlotsType SlotType;

    public GameObject item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }


    

    //chamado quando soltamos o objeto em cima de outro objeto
    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragItem.ItemBeginDragged.GetComponent<DragItem>().SetParent(transform, this);

        }
    }


}