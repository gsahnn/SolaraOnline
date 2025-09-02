// MonsterController.cs
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("Canavar Ayarlarý")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Loot Ayarlarý")]
    [SerializeField] private LootTable lootTable; // Bu canavar hangi eþya tablosunu kullanacak?
    [SerializeField] private GameObject itemPickupPrefab; // Yere düþecek eþyanýn prefab'ý

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Dýþarýdan hasar almamýzý saðlayacak public bir fonksiyon.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " " + damage + " hasar aldý. Kalan can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " öldü.");

        // Eþya Düþürme Mantýðý
        DropLoot();

        // Canavar objesini yok et.
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        // Eðer bir loot tablosu atanmýþsa...
        if (lootTable != null)
        {
            // Tablodaki her bir olasý eþya için...
            foreach (var lootItem in lootTable.possibleLoot)
            {
                // Rastgele bir sayý (0-100) üret ve eþyanýn düþme þansýndan küçük mü diye kontrol et.
                float randomChance = Random.Range(0f, 100f);
                if (randomChance <= lootItem.dropChance)
                {
                    // Þans tuttuysa, eþyayý oluþtur.
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

            // Yaratýlan objenin ItemPickup script'ine ulaþ.
            ItemPickup pickupScript = droppedItemObject.GetComponent<ItemPickup>();

            // Ve ona hangi eþyayý, kaç adet taþýyacaðýný söyle.
            pickupScript.Initialize(itemDataToDrop, 1); // Þimdilik 1 adet düþürüyoruz.
        }
    }
}

   