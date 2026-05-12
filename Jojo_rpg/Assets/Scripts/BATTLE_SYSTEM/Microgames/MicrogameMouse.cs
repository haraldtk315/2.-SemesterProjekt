using System.Xml.Serialization;
using UnityEngine;

public class MicrogameMouse : MonoBehaviour
{
    public MICROGAMEHANDLER microgameHandler;

    private void DisableMouse()
    {
        this.gameObject.SetActive(false);
    }

    private void InitiateSyringeSequence()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        microgameHandler.EnableSyringe();
        EnableClock();
        DisableMouse();
    }

    private void InitiateRapidPunchSequence()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        microgameHandler.EnableRapidPunch();
        EnableClock();
        DisableMouse();
    }

    private void EnableClock()
    {
        microgameHandler.EnableClock();
    }
}
