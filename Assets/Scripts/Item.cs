using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string Name;
    public float Value;

    [System.Serializable]
    public enum Type
    {
        Potion,
        Elixir,
        Crystal,

    }

    public Type ItemType;

    [System.Serializable]
    public enum SlotsType
    {
        Helmet,
        Shield,
        Armor,
        Trincket,
        Sword,

    }

    public SlotsType SlotType;

    public Player player;

    public void GetAction()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


        switch (ItemType)
        {
            case Type.Potion:
                Debug.Log("Health + " + Value);
                player.IncreaseStats(Value,0f);
                break;

            case Type.Elixir:
                Debug.Log("Elixir + " + Value);
                player.IncreaseStats(0f, Value);
                break;

            case Type.Crystal:
                Debug.Log("Crystal + " + Value);
                player.IncreaseStats(Value, Value);
                break;
        }
    }

    public void RemoveStats()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


        switch (ItemType)
        {
            case Type.Potion:
                Debug.Log("Health + " + Value);
                player.DecreaseStats(Value, 0f);
                break;

            case Type.Elixir:
                Debug.Log("Elixir + " + Value);
                player.DecreaseStats(0f, Value);
                break;

            case Type.Crystal:
                Debug.Log("Crystal + " + Value);
                player.DecreaseStats(Value, Value);
                break;
        }
    }


}
