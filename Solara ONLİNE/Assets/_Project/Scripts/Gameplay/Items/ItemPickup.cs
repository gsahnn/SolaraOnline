// ItemPickup.cs
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int amount = 1;

    // YEN� FONKS�YON: Bu fonksiyon, obje yarat�ld�ktan sonra �a�r�larak
    // i�indeki e�ya verisini ayarlamam�z� sa�lar.
    public void Initialize(ItemData item, int pickupAmount)
    {
        this.itemData = item;
        this.amount = pickupAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Objeye temas edenin bir "PlayerInventory" bile�eni var m� diye kontrol et.
        // Bu, sadece oyuncunun e�yalar� toplayabilmesini sa�lar.
        if (other.TryGetComponent(out PlayerInventory playerInventory))
        {
            // E�er oyuncunun envanterine bu e�yay� ekleyebildiysek...
            if (playerInventory.inventory.AddToInventory(itemData, amount))
            {
                // Toplama i�lemi ba�ar�l� olduysa, bu objeyi d�nyadan yok et.
                Destroy(gameObject);
            }
            else
            {
                // Envanter doluysa, konsola bir mesaj yazd�rabiliriz (opsiyonel).
                Debug.Log("Envanter Dolu!");
            }
        }
    }
}