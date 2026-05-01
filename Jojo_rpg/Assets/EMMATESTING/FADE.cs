using UnityEngine;
using UnityEngine.UI;

public class FADE : MonoBehaviour
{
    public float alpha = 0;
    public Image Fade;

    private void Start()
    {
        Fade = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        alpha += Time.deltaTime / 100;

        Fade.color = new Color(0, 0, 0, alpha);
    }
}
