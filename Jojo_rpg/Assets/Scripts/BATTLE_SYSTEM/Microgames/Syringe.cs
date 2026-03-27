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

    private bool hit;
    private RectTransform arteryTransform;

    private void Awake()
    {
        arteryTransform = artery.GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void OnEnable()
    {
        hit = false;
        arteryTransform.localPosition = new Vector3(Random.Range(-250, 250), -135);
        StartCoroutine(EndMicrogame());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator EndMicrogame()
    {
        yield return new WaitForSeconds(microgameTime);
        if (hit)
        {

        }
        MICROGAMEHANDLER.instance.EndMicrogame(type, 0);
    }
}
