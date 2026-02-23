using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TITLE : MonoBehaviour
{
    [SerializeField] private string BattleSystem = "FIGHT";

    public void TestBattleSystem()
    {
        SceneManager.LoadScene(BattleSystem);
    }
}

