using System.Collections;
using UnityEngine;

public class RapidPunch : MonoBehaviour
{
    public MICROGAMEHANDLER MH;

    public MICROGAMEHANDLER.MICROGAMES type;
    public int damagePerPunch;
    public float microgameTime;

    private int score;
    private bool leftPunch;

    private void OnEnable()
    {
        score = 0;
        leftPunch = true;
        StartCoroutine(EndMicrogame());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && leftPunch)
        {
            score += 1;
            leftPunch = false;
        }
        if (Input.GetKeyDown(KeyCode.D) && !leftPunch)
        {
            score += 1;
            leftPunch = true;
        }
    }

    private IEnumerator EndMicrogame()
    {
        yield return new WaitForSeconds(microgameTime);
        MH.EndMicrogame(type, score * damagePerPunch);
    }

}
