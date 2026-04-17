using TMPro;
using UnityEngine;

public class INFO_BOX : MonoBehaviour
{
    public TextMeshProUGUI TEXT;
    public RectTransform BackgroundTransform;

    private void Awake()
    {
        SHOW("IT WORKS? LIKE IT ACTUALLY WORKS, no kidding... It really does work. I don't know what to say. AHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
        HIDE();
    }

    private void Update()
    {

    }

    private void SHOW(string INFO)
    {
        gameObject.SetActive(true);

        TEXT.text = INFO;
    }

    private void HIDE()
    {
        gameObject.SetActive(false);
    }

}
