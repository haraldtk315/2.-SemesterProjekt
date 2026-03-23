using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string displayName;
    public Sprite icon;
    public ItemType Type;
    public int value;

    public enum ItemType
    {
        health,
        focus,
        revive
    }

    [TextArea]
    public string description;
}