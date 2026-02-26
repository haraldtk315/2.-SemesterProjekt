using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TITLE : MonoBehaviour
{
    private const string BATTLESYSTEM = "FIGHT";
    private const string OVERWORLD = "OVERWORLD";

    public void TestBattleSystem()
    {
        SceneManager.LoadScene(BATTLESYSTEM);
    }

    public void TestOverworldSystem()
    {
        SceneManager.LoadScene(OVERWORLD);
    }
   
}

