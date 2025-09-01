// ItemPickup.cs
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // Bu obje hangi e�yan�n verisini ta��yor?
    [SerializeField] private int amount = 1;    // Bu e�yadan ka� adet var?

    // OnTriggerEnter fonksiyonu, bir Collider'�n "Is Trigger" i�aretli ba�ka bir Collider'a
    // girdi�i anda otomatik olarak �al���r.
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