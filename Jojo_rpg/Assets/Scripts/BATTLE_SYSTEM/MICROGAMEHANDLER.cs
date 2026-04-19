using System;
using UnityEngine;


public class MICROGAMEHANDLER : MonoBehaviour
{
    public static MICROGAMEHANDLER instance;

    public enum MICROGAMES
    {
        RAPID_PUNCH,

        SYRINGE,
        BEER_POUR,

        SPEECH
    }

    public BATTLEHANDLER BH;
    public GameObject rapidPunchMicrogame;
    public GameObject rapidPunchText;
    public GameObject syringeMicrogame;
    public GameObject speechMicrogame;
    public GameObject speechText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Turns off all microgames on boot
        rapidPunchMicrogame.SetActive(false);
        rapidPunchText.SetActive(false);
        syringeMicrogame.SetActive(false);
        speechMicrogame.SetActive(false);
        speechText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMicrogame(MICROGAMES microgame)
    {
        if (microgame == MICROGAMES.RAPID_PUNCH)
        {
            rapidPunchMicrogame.SetActive(true);
            rapidPunchText.SetActive(true);
        }

        if (microgame == MICROGAMES.SYRINGE)
        {
            syringeMicrogame.SetActive(true);
        }

        if (microgame == MICROGAMES.SPEECH)
        {
            speechMicrogame.SetActive(true);
            speechText.SetActive(true);
        }
    }

    public void EndMicrogame(MICROGAMES microgame, int damage, int acc = 100, int focus = 0, float buff = 1)
    {
        if (microgame == MICROGAMES.RAPID_PUNCH)
        {
            rapidPunchMicrogame.SetActive(false);
            rapidPunchText.SetActive(false);
        }

        if (microgame == MICROGAMES.SYRINGE)
        {
            syringeMicrogame.SetActive(false);
        }

        if (microgame == MICROGAMES.SPEECH)
        {
            speechMicrogame.SetActive(false);
            speechText.SetActive(false);
        }

        BH.CURRENT_STATE = BATTLEHANDLER.STATEMACHINE.BATTLE;
        BH.StateMachine(BATTLEHANDLER.STATEMACHINE.BATTLE);
        BH.TARGET_ATTACK(BH.ORDER[BH.ON_CURRENT_CHAMP], BH.TARGET_ENEMY, damage, acc, focus, buff);
    }


}
