using UnityEngine;
using UnityEngine.SceneManagement;

public class MUSICMANAGER : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip menuMusic;
    public AudioClip IntroTheme;
    public AudioClip OverWorldTheme;
    public AudioClip battleTheme;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "TITLE")
        {
            ChangeMusic(menuMusic);
        }
        else if (sceneName == "OVERWORLD")
        {
            ChangeMusic(OverWorldTheme);
        }
        else if (sceneName == "FIGHT")
        {
            ChangeMusic(battleTheme);
        }
        else if (sceneName == "Intro" || sceneName == "INTRO_2" || sceneName == "INTRO_3")
        {
            ChangeMusic(IntroTheme);
        }
    }

    private void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip)
            return;

        audioSource.clip = newClip;
        audioSource.Play();
    }
}
