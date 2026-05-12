using UnityEngine;

public class PartyObstacle : MonoBehaviour, IInteractable
{
    public string obstacleID;
    public string requiredPartyMemberID = "jack";

    public string[] blockedMessage;
    public string[] clearedMessage;

    [Header("Movement")]
    public float gridSize = 1f;

    private bool hasMoved = false;

    private void Start()
    {
        if (!string.IsNullOrEmpty(obstacleID) &&
            GAMEMANAGER.instance.clearedObstacles.Contains(obstacleID))
        {
            hasMoved = true;
            enabled = false;
        }
    }

    public void Interact(PlayerInteract player)
    {
        if (hasMoved)
            return;

        if (HasPartyMember(requiredPartyMemberID))
        {
            hasMoved = true;

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
                    null,
                    null,
                    null,
                    false,
                    gameObject
                );
            }
            else
            {
                ClearObstacle();
            }
        }
        else
        {
            if (blockedMessage != null && blockedMessage.Length > 0)
            {
                DIALOGUEHANDLER.instance.DialogueStart(
                    blockedMessage,
                    player.gameObject,
                    null,
                    null,
                    null,
                    null,
                    false,
                    gameObject
                );
            }
        }
    }

    public void ClearObstacle()
    {
        transform.position += new Vector3(0f, gridSize, 0f);

        enabled = false;
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

            if (info.champID == memberID)
                return true;
        }

        return false;
    }
}