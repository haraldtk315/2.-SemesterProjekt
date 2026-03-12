using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string displayName;
    public Sprite icon;

    [TextArea]
    public string description;
}