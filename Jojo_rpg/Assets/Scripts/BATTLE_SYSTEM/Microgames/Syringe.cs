using JetBrains.Annotations;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Syringe : MonoBehaviour
{
    public MICROGAMEHANDLER MH;
    public TextMeshProUGUI microgameClockTimer;

    public MICROGAMEHANDLER.MICROGAMES type;
    public GameObject syringe;
    public GameObject artery;
    public float microgameTime;

    public float syringeSpeed;

    private bool microgameActive;
    private float timeRemaining;
    private int damage;
    private bool hit;
    private bool syringeMovingLeft;
    private bool decend;
    private Coroutine endMicrogameRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void OnEnable()
    {
        microgameActive = false;
        hit = false;
        artery.transform.localPosition = new Vector3(Random.Range(-0.65f, 0.65f), 0.4f, -8f);
        syringe.transform.localPosition = new Vector3(0, 1.6f, syringe.transform.localPosition.z);
        decend = false;
        timeRemaining = microgameTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (microgameActive)
        {
            timeRemaining -= Time.deltaTime;
            microgameClockTimer.text = $"{Mathf.CeilToInt(timeRemaining)}";
            if (Input.GetKeyDown(KeyCode.Space) && !decend || Input.GetMouseButtonDown(0) && !decend)
            {
                decend = true;
                if ((artery.transform.localPosition.x - 0.15f) < syringe.transform.localPosition.x && syringe.transform.localPosition.x < (artery.transform.localPosition.x + 0.15f))
                {
                    hit = true;
                    Debug.Log("Hit!");
                }
                StopCoroutine(endMicrogameRoutine);
                StartCoroutine(EndMicrogame(0.5f));
            }

            if (decend && syringe.transform.localPosition.y > 0.7f)
            {
                syringe.transform.localPosition -= new Vector3(0, syringeSpeed * 2, 0);
            }

            if (syringe.transform.localPosition.x > 1 && !decend)
            {
                syringeMovingLeft = true;
            }

            if (syringe.transform.localPosition.x < -1 && !decend)
            {
                syringeMovingLeft = false;
            }

            if (syringeMovingLeft && !decend)
            {
                syringe.transform.localPosition += new Vector3(-syringeSpeed, 0, 0);
            }

            else if (!syringeMovingLeft && !decend)
            {
                syringe.transform.localPosition += new Vector3(syringeSpeed, 0, 0);
            }
        }
    }

    private void StartMicrogame()
    {
        endMicrogameRoutine = StartCoroutine(EndMicrogame(microgameTime));
        microgameActive = true;
    }

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }

    private IEnumerator EndMicrogame(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        if (hit)
        {
            damage = -35;
        }
        else
        {
            damage = 15;
        }
        microgameActive = false;
        MH.EndMicrogame(type, damage);
    }
}
