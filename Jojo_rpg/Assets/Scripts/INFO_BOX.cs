using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INFO_BOX : MonoBehaviour
{
    public TextMeshProUGUI TEXT;
    public RectTransform BackgroundTransform;
    public RectTransform Content;

    public float mousepos_x;
    public float mousepos_y;
    public float width;

    public float wait_time;
    public float time;

    private void Start()
    {
        TEXT.text = string.Empty;
    }

    private void Update()
    {
        mousepos_x = Input.mousePosition.x;
        mousepos_y = Input.mousePosition.y;
        width = Screen.width / 2;

        transform.position = new Vector2(mousepos_x, mousepos_y);
        BackgroundTransform.sizeDelta = new Vector2(TEXT.preferredWidth, BackgroundTransform.sizeDelta.y);

        if (mousepos_x <= width)
        {
            Content.localPosition = new Vector2(BackgroundTransform.sizeDelta.x / 2 + 5, BackgroundTransform.sizeDelta.y / 2 + 5);
        }
        else
        {
            Content.localPosition = new Vector2(-(BackgroundTransform.sizeDelta.x / 2 + 5), BackgroundTransform.sizeDelta.y / 2 + 5);
        }
    }

    public void SHOW(string Text, bool ON_UI)
    {
        if (ON_UI)
        {
            time = 0;
        }

        time += Time.deltaTime;

        if (time > wait_time)
        {
            TEXT.text = Text;
        }
    }

    public void HIDE()
    {
        time = 0;
        TEXT.text = string.Empty;
    }

    public void INSTANT_SHOW(string Text)
    {
        TEXT.text = Text;
    }
}
