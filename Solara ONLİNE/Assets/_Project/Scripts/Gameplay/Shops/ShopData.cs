// ShopData.cs
using UnityEngine;
using System.Collections.Generic;

// Bu, d�kkandaki tek bir sat�l�k e�yay� temsil eder.
[System.Serializable]
public class ShopItem
{
    public ItemData item;
    public int price; // E�er price 0 ise, e�yan�n kendi varsay�lan fiyat� kullan�l�r (ileride ekleyebiliriz).
}

[CreateAssetMenu(fileName = "New Shop", menuName = "Solara/Data/Shop")]
public class ShopData : ScriptableObject
{
    public List<ShopItem> itemsForSale;
}