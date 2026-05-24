using System.Xml.Serialization;
using UnityEngine;

public class MicrogameMouse : MonoBehaviour
{
    public MICROGAMEHANDLER microgameHandler;
    public RectTransform rectTransform;

    private void DisableMouse()
    {
        this.gameObject.SetActive(false);
    }

    private void InitiateSyringeSequence()
    {
        rectTransform.anchoredPosition = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        microgameHandler.EnableSyringe();
        EnableClock();
        DisableMouse();
    }

    private void InitiateRapidPunchSequence()
    {
        rectTransform.anchoredPosition = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        microgameHandler.EnableRapidPunch();
        EnableClock();
        DisableMouse();
    }

    private void InitiateSpeechSequence()
    {
        rectTransform.anchoredPosition = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        microgameHandler.EnableSpeech();
        EnableClock();
        DisableMouse();
    }

    private void EnableClock()
    {
        microgameHandler.EnableClock();
    }
}
