// ItemPickup.cs
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int amount = 1;

    // YENÝ FONKSÝYON: Bu fonksiyon, obje yaratýldýktan sonra çaðrýlarak
    // içindeki eþya verisini ayarlamamýzý saðlar.
    public void Initialize(ItemData item, int pickupAmount)
    {
        this.itemData = item;
        this.amount = pickupAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Objeye temas edenin bir "PlayerInventory" bileþeni var mý diye kontrol et.
        // Bu, sadece oyuncunun eþyalarý toplayabilmesini saðlar.
        if (other.TryGetComponent(out PlayerInventory playerInventory))
        {
            // Eðer oyuncunun envanterine bu eþyayý ekleyebildiysek...
            if (playerInventory.inventory.AddToInventory(itemData, amount))
            {
                // Toplama iþlemi baþarýlý olduysa, bu objeyi dünyadan yok et.
                Destroy(gameObject);
            }
            else
            {
                // Envanter doluysa, konsola bir mesaj yazdýrabiliriz (opsiyonel).
                Debug.Log("Envanter Dolu!");
            }
        }
    }
}