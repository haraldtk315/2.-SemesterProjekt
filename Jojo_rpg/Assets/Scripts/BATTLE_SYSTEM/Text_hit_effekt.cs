using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Text_hit_effekt : MonoBehaviour
{
    public TextMeshPro TMP;
    const string MISS = "MISS!";

    public float height_change = 10;
    public float side_step = 0;
    public float fall_speed = 3;

    public float alpha = 1;

    public Color current_color;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TMP = GetComponent<TextMeshPro>();
    }

    private void Awake()
    {
        TMP = GetComponent<TextMeshPro>();
        transform.position += new Vector3(0, 0, -1);
        side_step += Random.Range(-1.5f, 1.5f);

        current_color = TMP.color;
    }

    // Update is called once per frame
    void Update()
    {
        height_change += Random.Range(-2.5f, 2.5f) * Time.deltaTime;

        transform.position += new Vector3(side_step, height_change, 0) * Time.deltaTime;
        height_change -= fall_speed * Time.deltaTime;

        alpha -= Time.deltaTime / 2.25f;
        
        current_color.a = alpha;
        TMP.color = current_color;

        if (alpha <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Miss()
    {
        TMP.text = MISS;
    }

    public void hit(int Damage = 0, bool Overkill = false)
    {
        TMP.text = Damage.ToString();

        if (Overkill)
        {
            TMP.color = Color.red;
            TMP.text += "!";
            TMP.fontStyle = TMPro.FontStyles.Bold;
        }
    }
}
