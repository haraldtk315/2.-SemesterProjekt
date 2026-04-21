using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Speech : MonoBehaviour
{
    public MICROGAMEHANDLER MH;

    public MICROGAMEHANDLER.MICROGAMES type;
    public TextMeshProUGUI textUI;
    public float microgameTime;
    

    private string remainingSentence = string.Empty;
    private int score;
    private float buff;
    private Dictionary<int, string> sentenceBank = new Dictionary<int, string>{ {1, "Stand proud soldiers!"}, { 2, "Remember what's at stake!" }, {3, "London will be free!" }, {4, "We must win this fight!" } };
    private Coroutine endMicrogameRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        score = 0;
        buff = 1;
        endMicrogameRoutine = StartCoroutine(EndMicrogame(microgameTime));
        SetRemainingSentence(sentenceBank[UnityEngine.Random.Range(1, sentenceBank.Count + 1)]);
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

            if (letter.Length == 1)
            {
                if (remainingSentence.IndexOf(letter) == 0)
                {
                    LetterTyped();
                    if (remainingSentence.Length == 0)
                    {
                        StopCoroutine(endMicrogameRoutine);
                        StartCoroutine(EndMicrogame(0.2f));
                    }
                }
            }
        }
    }

    private void LetterTyped()
    {
        SetRemainingSentence(remainingSentence.Remove(0, 1));
        score += 1;

    }

    private void SetRemainingSentence(string inputString)
    {
        remainingSentence = inputString;
        textUI.text = remainingSentence;
    }

    private IEnumerator EndMicrogame(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        if (remainingSentence.Length == 0)
        {
            buff = 1.75f;
        }
        else if (score < 12)
        {
            buff = 1.2f;
        }
        else
        {
            buff = 1.35f;
        }

        Debug.Log($"{score}, {buff}");

        MH.EndMicrogame(type, 0, 100, 0, buff);
        }
    }



