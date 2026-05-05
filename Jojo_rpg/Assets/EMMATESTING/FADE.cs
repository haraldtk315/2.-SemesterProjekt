using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FADE : MonoBehaviour
{
    public float alpha = 0;
    public Image Fade;
    public TextMeshProUGUI TEXT;

    private void Start()
    {
        Fade = GetComponent<Image>();
    }

    // Update is called once per frame

    private void Awake()
    {
        Fade.color = new Color(0, 0, 0, 0);

        TEXT.color = new Color(255, 255, 255, 0);
    }
    void Update()
    {
        alpha += Time.deltaTime / 1;

        Fade.color = new Color(0, 0, 0, alpha);

        TEXT.color = new Color(255, 255, 255, alpha);
    }
}
