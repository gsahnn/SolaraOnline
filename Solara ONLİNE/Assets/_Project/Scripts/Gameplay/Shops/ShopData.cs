// ShopData.cs
using UnityEngine;
using System.Collections.Generic;

// Bu, dükkandaki tek bir satýlýk eþyayý temsil eder.
[System.Serializable]
public class ShopItem
{
    public ItemData item;
    public int price; // Eðer price 0 ise, eþyanýn kendi varsayýlan fiyatý kullanýlýr (ileride ekleyebiliriz).
}

[CreateAssetMenu(fileName = "New Shop", menuName = "Solara/Data/Shop")]
public class ShopData : ScriptableObject
{
    public List<ShopItem> itemsForSale;
}