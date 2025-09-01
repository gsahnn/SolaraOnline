// ItemPickup.cs
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // Bu obje hangi eþyanýn verisini taþýyor?
    [SerializeField] private int amount = 1;    // Bu eþyadan kaç adet var?

    // OnTriggerEnter fonksiyonu, bir Collider'ýn "Is Trigger" iþaretli baþka bir Collider'a
    // girdiði anda otomatik olarak çalýþýr.
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