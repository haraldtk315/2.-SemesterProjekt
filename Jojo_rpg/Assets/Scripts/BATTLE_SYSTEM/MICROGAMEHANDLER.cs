using System;
using UnityEngine;


public class MICROGAMEHANDLER : MonoBehaviour
{
    public enum MICROGAMES
    {
        RAPID_PUNCH,

        SYRINGE,
        BEER_POUR,

        SPEECH
    }

    public BATTLEHANDLER BH;

    public GameObject microgameClock;
    public Animator microgameClockAnimationController;

    public GameObject microgameMouse;
    public Animator microgameMouseAnimationController;

    public GameObject rapidPunchMicrogame;
    public GameObject rapidPunchText;

    public GameObject syringeMicrogame;
    public Animator syringeAnimationController;
    public GameObject SyringeText;

    public GameObject speechMicrogame;
    public GameObject speechText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableMicrogames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableMicrogames()
    {
        // Turns off all microgames
        rapidPunchMicrogame.SetActive(false);
        rapidPunchText.SetActive(false);
        syringeMicrogame.SetActive(false);
        speechMicrogame.SetActive(false);
        speechText.SetActive(false);
        SyringeText.SetActive(false);
    }

    public void StartMicrogame(MICROGAMES microgame)
    {
        if (microgame == MICROGAMES.RAPID_PUNCH)
        {
            microgameMouse.SetActive(true);
            microgameMouseAnimationController.Play("MouseEnter");
            rapidPunchText.SetActive(true);
        }

        if (microgame == MICROGAMES.SYRINGE)
        {
            microgameMouse.SetActive(true);
            microgameMouseAnimationController.Play("MouseEnter 0");
            SyringeText.SetActive(true);
        }

        if (microgame == MICROGAMES.SPEECH)
        {
            speechMicrogame.SetActive(true);
            speechText.SetActive(true);
        }
    }

    public void EndMicrogame(MICROGAMES microgame, int damage, int acc = 100, int focus = 0, float buff = 1)
    {
        microgameClockAnimationController.Play("ClockExit");

        if (microgame == MICROGAMES.RAPID_PUNCH)
        {
            rapidPunchMicrogame.SetActive(false);
        }

        if (microgame == MICROGAMES.SYRINGE)
        {
            syringeAnimationController.Play("SyringeExit");
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

    public void EnableClock()
    {
        microgameClock.SetActive(true);
        microgameClockAnimationController.Play("ClockEnter");
    }

    public void EnableRapidPunch()
    {
        rapidPunchMicrogame.SetActive(true);
        rapidPunchText.SetActive(false);
    }

    public void EnableSyringe()
    {
        syringeMicrogame.SetActive(true);
        syringeAnimationController.Play("SyringeEnter");
        SyringeText.SetActive(false);
    }
}
