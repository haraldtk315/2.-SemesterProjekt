using UnityEngine;
using System.Collections;

public class OVERWORLDRESTORE : MonoBehaviour
{

    
    private IEnumerator Start()
    {
        
        if(GAMEMANAGER.instance == null)
            yield break;

        if (!GAMEMANAGER.instance.shouldRestorePlayer) yield break;

        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null) 
        {
            player.transform.position = GAMEMANAGER.instance.returnPlayerPosition;

            PlayerInteract interact = player.GetComponent<PlayerInteract>();
            if (interact != null)
            {
                interact.facing = GAMEMANAGER.instance.returnPlayerFacing;
            }

        }
        GAMEMANAGER.instance.shouldRestorePlayer = false;
    }
}
