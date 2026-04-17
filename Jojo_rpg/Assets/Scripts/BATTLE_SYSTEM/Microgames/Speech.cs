using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speech : MonoBehaviour
{
    public MICROGAMEHANDLER.MICROGAMES type;
    public TextMeshProUGUI textUI;
    public float microgameTime;
    

    private string remainingSentence = string.Empty;
    private Dictionary<int, string> sentenceBank = new Dictionary<int, string>{ {1, "This is the first one"}, { 2, "This is the second one" } };

    private Coroutine endMicrogameRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        endMicrogameRoutine = StartCoroutine(EndMicrogame(microgameTime));
        SetRemainingSentence(sentenceBank[UnityEngine.Random.Range(0, sentenceBank.Count)]);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            string letter = Input.inputString;

            Debug.Log(letter);

            if (letter.Length == 1)
            {
                if (remainingSentence.IndexOf(letter) == 0)
                {
                    LetterTyped();
                }
            }
        }
    }

    private void LetterTyped()
    {
        SetRemainingSentence(remainingSentence.Remove(0, 1));
    }

    private void SetRemainingSentence(string inputString)
    {
        remainingSentence = inputString;
        textUI.text = remainingSentence;
    }

    private IEnumerator EndMicrogame(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        MICROGAMEHANDLER.instance.EndMicrogame(0, 0);
    }

}

