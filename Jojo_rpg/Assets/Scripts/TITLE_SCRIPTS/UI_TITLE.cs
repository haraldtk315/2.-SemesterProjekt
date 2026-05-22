using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TITLE : MonoBehaviour
{
    private const string BATTLESYSTEM = "FIGHT";
    private const string OVERWORLD = "OVERWORLD";
    private const string DIALOGUE = "DIALOGUE";
    private const string INTRO = "INTRO";
    private const string PROLOGUE = "PROLOGUE";

    public void TestBattleSystem()
    {
        SceneManager.LoadScene(BATTLESYSTEM);
    }

    public void TestOverworldSystem()
    {
        SceneManager.LoadScene(OVERWORLD);
    }

    public void ToINTROWORLD()
    {
        SceneManager.LoadScene(INTRO);
    }
   
    public void TestDIALOGUE()
    {
        SceneManager.LoadScene(DIALOGUE);
    }

    public void TestPROLOGUE()
    {
        SceneManager.LoadScene(PROLOGUE);
    }
}

