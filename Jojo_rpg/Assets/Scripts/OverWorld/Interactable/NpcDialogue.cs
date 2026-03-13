using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string[] message;

    public GameObject[] enemies;

    public void Start()
    {
       
    }
    public void Interact(PlayerInteract player)
    {
        GameObject.FindGameObjectWithTag("DH").GetComponent<DIALOGUEHANDLER>().ENEMIES = enemies;
        DIALOGUEHANDLER.instance.DialogueStart(message, player.gameObject);
    }

}