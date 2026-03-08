using UnityEngine;

[CreateAssetMenu(fileName = "BASIC_ATTACKS", menuName = "Scriptable Objects/BASIC_ATTACKS")]
public class BASIC_ATTACKS : ScriptableObject
{
    public string name;
    public int damage;
    public int acc;

    [TextArea]
    [SerializeField] string Description;

    public string Name
    {
        get { return name; }
    }

    public string Move_info
    {
        get { return Description; }
    }

    public int Damage
    {
        get { return damage; }
    }
}
