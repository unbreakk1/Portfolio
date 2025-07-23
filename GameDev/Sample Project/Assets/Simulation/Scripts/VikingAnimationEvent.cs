using UnityEngine;

public class VikingAnimationEvent : MonoBehaviour
{
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private ItemSpawn itemSpawn;
    [SerializeField] private ResourceObject resources;
    [SerializeField, Range(1, 50)] private float resourceCost;

    [SerializeField] private int workCycle;

    public void ReduceResources()
    {
        resources.IronAmount -= resourceCost;
         workCycle++;

        if (workCycle == 4 && itemSpawn.ItemList.Count <= 0)
           SpawnItem();
    }
    
    private void SpawnItem()
    {
        swordPrefab.gameObject.SetActive(true);
        itemSpawn.ItemList.Add(swordPrefab);
        workCycle = 0;
    }
}