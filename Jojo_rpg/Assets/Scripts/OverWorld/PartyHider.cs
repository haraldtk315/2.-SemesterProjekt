using UnityEngine;

public class PartyHider : MonoBehaviour
{
    public GameObject templateToCheck;
    public SpriteRenderer sprite;
    public string boulderID;

    void Start()
    {
        if (GAMEMANAGER.instance.clearedObstacles.Contains(boulderID))
        {
            sprite.enabled = true;
        }
    }
}
        
    

