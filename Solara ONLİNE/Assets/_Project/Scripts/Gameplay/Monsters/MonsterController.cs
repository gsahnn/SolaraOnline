using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class MonsterController : MonoBehaviour
{
    [Header("Loot Ayarlar�")]
    [SerializeField] private LootTable lootTable;
    [SerializeField] private GameObject itemPickupPrefab;
    [SerializeField] private int experienceGranted = 50;

    // Bu fonksiyon, CharacterStats script'i taraf�ndan, canavar�n can� 0 oldu�unda �a�r�l�r.
    public void HandleDeath(CharacterStats killer)
    {
        if (killer != null)
        {
            // �ld�rene tecr�be puan�n� ver.
            killer.AddExperience(experienceGranted);

            // �ld�renin bir QuestLog'u varsa, g�rev ilerlemesi i�in ona haber ver.
            // D�ZELTME: 'if' ifadesinin sonundaki noktal� virg�l kald�r�ld�.
            if (killer.TryGetComponent(out QuestLog questLog))
            {
                // Bu canavar�n ad�n� ve 1 adet kesildi�ini bildir.
                // �rne�in, canavar�n ad� "Kurt" ise, "Kurt" ismini g�nderir.
                questLog.AddQuestProgress(this.gameObject.name, 1);
            }
        }

        // E�yalar� d���r.
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