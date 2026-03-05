using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TITLE : MonoBehaviour
{
    private const string BATTLESYSTEM = "FIGHT";
    private const string OVERWORLD = "OVERWORLD";
    private const string DIALOGUE = "DIALOGUE";

    public void TestBattleSystem()
    {
        SceneManager.LoadScene(BATTLESYSTEM);
    }

    public void TestOverworldSystem()
    {
        SceneManager.LoadScene(OVERWORLD);
    }
   
    public void TestDIALOGUE()
    {
        SceneManager.LoadScene(DIALOGUE);
    }
}

