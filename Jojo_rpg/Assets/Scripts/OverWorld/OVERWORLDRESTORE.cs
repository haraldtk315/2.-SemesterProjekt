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
        GameObject jack = GameObject.FindGameObjectWithTag("Jack");
        GameObject werner = GameObject.FindGameObjectWithTag("Werner");

        if (player != null) 
        {
            player.transform.position = GAMEMANAGER.instance.returnPlayerPosition;

            PlayerInteract interact = player.GetComponent<PlayerInteract>();
            if (interact != null)
            {
                interact.facing = GAMEMANAGER.instance.returnPlayerFacing;
            }

        }

        if (jack != null)
        {
            jack.transform.position = GAMEMANAGER.instance.returnJackPosition;
        }

        if (werner != null)
        {
            werner.transform.position = GAMEMANAGER.instance.returnWernerPosition;
        }

        GAMEMANAGER.instance.shouldRestorePlayer = false;
    }
}
