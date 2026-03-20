using TMPro;
using UnityEngine;

public class Text_hit_effekt : MonoBehaviour
{
    public TextMeshPro TMP;
    const string MISS = "MISS!";

    public float height_change = 10;
    public float side_step = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TMP = GetComponent<TextMeshPro>();
    }

    private void Awake()
    {
        TMP = GetComponent<TextMeshPro>();
        transform.position += new Vector3(0, 0, -1);
        side_step += Random.Range(-0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        height_change += Random.Range(-0.1f, 0.1f);

        transform.position += new Vector3(side_step, height_change, 0);
        height_change -= 1 * Time.deltaTime;
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
            TMP.fontStyle = TMPro.FontStyles.Bold;
            TMP.text += "!";
        }
    }
}
