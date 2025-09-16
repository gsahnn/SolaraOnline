using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class MonsterController : MonoBehaviour
{
    [Header("Loot Ayarlarý")]
    [SerializeField] private LootTable lootTable;
    [SerializeField] private GameObject itemPickupPrefab;
    [SerializeField] private int experienceGranted = 50;

    // Bu fonksiyon, CharacterStats script'i tarafýndan, canavarýn caný 0 olduðunda çaðrýlýr.
    public void HandleDeath(CharacterStats killer)
    {
        if (killer != null)
        {
            // Öldürene tecrübe puanýný ver.
            killer.AddExperience(experienceGranted);

            // Öldürenin bir QuestLog'u varsa, görev ilerlemesi için ona haber ver.
            // DÜZELTME: 'if' ifadesinin sonundaki noktalý virgül kaldýrýldý.
            if (killer.TryGetComponent(out QuestLog questLog))
            {
                // Bu canavarýn adýný ve 1 adet kesildiðini bildir.
                // Örneðin, canavarýn adý "Kurt" ise, "Kurt" ismini gönderir.
                questLog.AddQuestProgress(this.gameObject.name, 1);
            }
        }

        // Eþyalarý düþür.
        DropLoot();

        // Son olarak, canavar objesini yok et.
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