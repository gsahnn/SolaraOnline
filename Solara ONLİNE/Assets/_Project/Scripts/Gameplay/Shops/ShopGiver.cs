// ShopGiver.cs
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class ShopGiver : MonoBehaviour
{
    [SerializeField] private ShopData shopData;

    public void OnInteract(GameObject interactor)
    {
        if (interactor.TryGetComponent(out PlayerInventory playerInventory))
        {
            ShopSystem.Instance.OpenShop(shopData, playerInventory);
        }
    }
}