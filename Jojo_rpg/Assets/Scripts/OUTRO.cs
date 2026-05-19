using UnityEngine;

public class OUTRO : MonoBehaviour
{
    public RectTransform trans;
    public float val;


    // Update is called once per frame
    void Update()
    {
        val += Time.deltaTime * 15f;
        trans.transform.localPosition = new Vector3(125, val, 0);
    }
}
