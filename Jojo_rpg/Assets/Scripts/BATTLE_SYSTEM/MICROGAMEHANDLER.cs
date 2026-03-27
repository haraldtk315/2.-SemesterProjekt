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
    }

    public GameObject rapidPunchMicrogame;
    public GameObject syringeMicrogame;

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
        syringeMicrogame.SetActive(false);
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
        }

        if (microgame == MICROGAMES.SYRINGE)
        {
            syringeMicrogame.SetActive(true);
        }
    }

    public void EndMicrogame(MICROGAMES microgame, int damage, int acc = 100, int focus = 0)
    {
        if (microgame == MICROGAMES.RAPID_PUNCH)
        {
            rapidPunchMicrogame.SetActive(false);
        }

        if (microgame == MICROGAMES.SYRINGE)
        {
            syringeMicrogame.SetActive(false);
        }

        BATTLEHANDLER.instance.CURRENT_STATE = BATTLEHANDLER.STATEMACHINE.BATTLE;
        BATTLEHANDLER.instance.StateMachine(BATTLEHANDLER.STATEMACHINE.BATTLE);
        BATTLEHANDLER.instance.TARGET_ATTACK(BATTLEHANDLER.instance.ORDER[BATTLEHANDLER.instance.ON_CURRENT_CHAMP], BATTLEHANDLER.instance.TARGET_ENEMY, damage, acc, focus);
    }


}
