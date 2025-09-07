// MonsterController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class MonsterController : MonoBehaviour
{
    [Header("Loot Ayarlarý")]
    [SerializeField] private LootTable lootTable;
    [SerializeField] private GameObject itemPickupPrefab;
    [SerializeField] private int experienceGranted = 50;

    public void HandleDeath(CharacterStats killer)
    {
        if (killer != null)
        {
            killer.AddExperience(experienceGranted);
        }

        DropLoot();
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        if (lootTable == null) return;

        foreach (var lootItem in lootTable.possibleLoot)
        {
            float randomChance = Random.Range(0f, 100f);
            if (randomChance <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemData);
            }
        }
    }

    private void InstantiateLoot(ItemData itemDataToDrop)
    {
        if (itemPickupPrefab != null)
        {
            GameObject droppedItemObject = Instantiate(itemPickupPrefab, transform.position, Quaternion.identity);
            ItemPickup pickupScript = droppedItemObject.GetComponent<ItemPickup>();
            pickupScript.Initialize(itemDataToDrop, 1);
        }
    }
}