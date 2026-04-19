using UnityEngine;

public class PartyObstacle : MonoBehaviour, IInteractable
{
    public string obstacleID;
    public string requiredPartyMemberID = "jack";

    public string[] blockedMessage;
    public string[] clearedMessage;

    private void Start()
    {
        if (!string.IsNullOrEmpty(obstacleID) &&
            GAMEMANAGER.instance.clearedObstacles.Contains(obstacleID))
        {
            Destroy(gameObject);
        }
    }

    public void Interact(PlayerInteract player)
    {
        if (HasPartyMember(requiredPartyMemberID))
        {
            if (!string.IsNullOrEmpty(obstacleID))
            {
                GAMEMANAGER.instance.clearedObstacles.Add(obstacleID);
            }

            if (clearedMessage != null && clearedMessage.Length > 0)
            {
                DIALOGUEHANDLER.instance.DialogueStart(
                    clearedMessage,
                    player.gameObject,
                    null,
                    gameObject,
                    null
                );
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (blockedMessage != null && blockedMessage.Length > 0)
            {
                DIALOGUEHANDLER.instance.DialogueStart(blockedMessage, player.gameObject);
            }
        }
    }

    private bool HasPartyMember(string memberID)
    {
        if (GAMEMANAGER.instance == null || GAMEMANAGER.instance.party == null)
            return false;

        for (int i = 0; i < GAMEMANAGER.instance.party.Length; i++)
        {
            GameObject member = GAMEMANAGER.instance.party[i];

            if (member == null) continue;

            CHAMP_INFO info = member.GetComponent<CHAMP_INFO>();
            if (info == null) continue;

            Debug.Log("Party slot " + i + ": " + info.Name + " / " + info.champID);

            if (info.champID == memberID)
                return true;
        }

        return false;
    }
}