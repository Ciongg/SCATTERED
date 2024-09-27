using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Scriptable object/ShopItem")]

public class ShopItem : ScriptableObject
{
  public Sprite image;
  public ShopItemType type;
  public int cost;
  public Rarity rarity;
  public ShopItemName itemName;
  

  
}

public enum ShopItemType{
  Seed,
  Character,
  Background,

}

public enum ShopItemName{
  Sunflower,
  Gaollium,
  Gerbaras,
  RareSeedBag,
  LegendarySeedBag,
}

public enum Rarity{
  Basic,
  Uncommon,
  Rare,
  Legendary,
}


