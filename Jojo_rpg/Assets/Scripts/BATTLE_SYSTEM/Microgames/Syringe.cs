using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Syringe : MonoBehaviour
{
    public MICROGAMEHANDLER.MICROGAMES type;
    public GameObject syringe;
    public GameObject artery;
    public float microgameTime;

    public int syringeSpeed;

    private bool hit;
    private bool syringeMovingLeft;
    private bool decend;
    private RectTransform arteryTransform;
    private RectTransform syringeTransform;
    private Coroutine endMicrogameRoutine;

    private void Awake()
    {
        arteryTransform = artery.GetComponent<RectTransform>();
        syringeTransform = syringe.GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void OnEnable()
    {
        hit = false;
        arteryTransform.localPosition = new Vector3(Random.Range(-250, 250), -135);
        syringeTransform.localPosition = Vector3.zero;
        decend = false;
        endMicrogameRoutine = StartCoroutine(EndMicrogame(microgameTime));
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            decend = true;
            StartCoroutine(Decend());
        }

        if (syringeTransform.localPosition.x > 300 && !decend)
        {
            syringeMovingLeft = true;
        }

        if (syringeTransform.localPosition.x < -300 && !decend)
        {
            syringeMovingLeft = false;
        }

        if (syringeMovingLeft && !decend)
        {
            syringeTransform.localPosition += new Vector3(-syringeSpeed, 0, 0);
        }

        else if (!syringeMovingLeft && !decend)
        {
            syringeTransform.localPosition += new Vector3(syringeSpeed, 0, 0);
        }
    }

    private IEnumerator Decend()
    {
        for (float t = 0f; t < 0.04f; t += Time.deltaTime)
        {
            syringeTransform.localPosition += new Vector3(0, -10, 0);
            yield return null; // "wait for a frame"
        }
        StopCoroutine(endMicrogameRoutine);
        StartCoroutine(EndMicrogame(0.5f));
        yield return decend = false;
    }

    private IEnumerator EndMicrogame(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        if (hit)
        {

        }
        MICROGAMEHANDLER.instance.EndMicrogame(type, 0);
    }
}
