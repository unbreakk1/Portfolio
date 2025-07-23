using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemList = new();

    public List<GameObject> ItemList => itemList;
}
