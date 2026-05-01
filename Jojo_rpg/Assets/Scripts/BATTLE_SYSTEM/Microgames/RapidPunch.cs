using System.Collections;
using TMPro;
using UnityEngine;

public class RapidPunch : MonoBehaviour
{
    public MICROGAMEHANDLER MH;
    public TextMeshProUGUI microgameClockTimer;

    public MICROGAMEHANDLER.MICROGAMES type;
    public int damagePerPunch;
    public float microgameTime;

    public Animator protagAnimator;

    private float timeRemaining;
    private int score;
    private bool leftPunch;

    private void OnEnable()
    {
        score = 0;
        leftPunch = true;
        StartCoroutine(EndMicrogame());
        timeRemaining = microgameTime;

    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        microgameClockTimer.text = $"{Mathf.CeilToInt(timeRemaining)}";
        if (Input.GetMouseButtonDown(0) && leftPunch)
        {
            protagAnimator.SetTrigger("LeftTrigger");
            score += 1;
            leftPunch = false;
        }
        if (Input.GetMouseButtonDown(1) && !leftPunch)
        {
            protagAnimator.SetTrigger("RightTrigger");
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
