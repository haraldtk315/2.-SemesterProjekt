using UnityEngine;

[CreateAssetMenu(fileName = "BASIC_ATTACKS", menuName = "Scriptable Objects/BASIC_ATTACKS")]
public class BASIC_ATTACKS : ScriptableObject
{
    public string name;
    public int damage;

    public bool SINGLE_TARGET;
}
