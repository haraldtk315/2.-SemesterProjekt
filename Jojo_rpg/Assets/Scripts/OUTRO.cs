using UnityEngine;

public class OUTRO : MonoBehaviour
{
    public RectTransform trans;
    public float val;
    public float textSpeed;


    // Update is called once per frame
    void Update()
    {
        val += Time.deltaTime * textSpeed;
        trans.transform.localPosition = new Vector3(125, val, 0);
    }
}
