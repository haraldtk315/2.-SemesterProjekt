using UnityEngine;

[CreateAssetMenu(fileName = "SPECIALS", menuName = "Scriptable Objects/SPECIALS")]
public class SPECIALS : BASIC_ATTACKS
{
    public int cost;

    public int Cost
    {
        get { return  cost; }
    }
}
