using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName ="Scriptable object/Item")]

public class Item : ScriptableObject
{
   public Sprite image;
   public ItemType type;
   public ActionType actionType;
   public bool stackable = true;

}

//creates drop downs
public enum ItemType{
    Seed,
    Tool
}

public enum ActionType{
    Plant,
    Recycle
}
