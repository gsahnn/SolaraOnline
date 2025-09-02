// MonsterController.cs
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("Canavar Ayarlar�")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Loot Ayarlar�")]
    [SerializeField] private LootTable lootTable; // Bu canavar hangi e�ya tablosunu kullanacak?
    [SerializeField] private GameObject itemPickupPrefab; // Yere d��ecek e�yan�n prefab'�

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // D��ar�dan hasar almam�z� sa�layacak public bir fonksiyon.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " " + damage + " hasar ald�. Kalan can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " �ld�.");

        // E�ya D���rme Mant���
        DropLoot();

        // Canavar objesini yok et.
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        // E�er bir loot tablosu atanm��sa...
        if (lootTable != null)
        {
            // Tablodaki her bir olas� e�ya i�in...
            foreach (var lootItem in lootTable.possibleLoot)
            {
                // Rastgele bir say� (0-100) �ret ve e�yan�n d��me �ans�ndan k���k m� diye kontrol et.
                float randomChance = Random.Range(0f, 100f);
                if (randomChance <= lootItem.dropChance)
                {
                    // �ans tuttuysa, e�yay� olu�tur.
                    InstantiateLoot(lootItem.itemData);
                }
            }
        }
    }

    private void InstantiateLoot(ItemData itemDataToDrop)
    {
        if (itemPickupPrefab != null)
        {
            GameObject droppedItemObject = Instantiate(itemPickupPrefab, transform.position, Quaternion.identity);

            // Yarat�lan objenin ItemPickup script'ine ula�.
            ItemPickup pickupScript = droppedItemObject.GetComponent<ItemPickup>();

            // Ve ona hangi e�yay�, ka� adet ta��yaca��n� s�yle.
            pickupScript.Initialize(itemDataToDrop, 1); // �imdilik 1 adet d���r�yoruz.
        }
    }
}

   