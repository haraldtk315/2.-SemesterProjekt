using UnityEngine;

[CreateAssetMenu(fileName = "SPECIALS", menuName = "Scriptable Objects/SPECIALS")]
public class SPECIALS : ScriptableObject
{
    public string name;
    public int damage;

    [TextArea]
    [SerializeField] string Description;

    public bool HIT_ALL;
}
