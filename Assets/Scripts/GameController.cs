using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject InventoryButton;
    public GameObject ItemPrefab;

    public static GameController instance;



    private void Awake()
    {
        instance = this;
    }

    public void ActiveGameObject(GameObject GO)
    {
        GO.SetActive(true);
        InventoryButton.SetActive(false);

    }

    public void DisableGameObject(GameObject GO)
    {
        GO.SetActive(false);
        InventoryButton.SetActive(true);

    }

}
