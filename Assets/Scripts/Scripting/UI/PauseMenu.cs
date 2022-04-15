using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button resume, quit;
    public bool isSinglePlayer;
    public bool isPaused;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        resume.onClick.AddListener(Resume);
        quit.onClick.AddListener(Quit);
    }

    public void Pause() {
        if(isSinglePlayer) {
            Time.timeScale = 0;
        }
        Debug.Log("amogus2");
        gameObject.SetActive(true);
    }

    void Resume() {
        if(isSinglePlayer) {
            Time.timeScale = 1;
        }
        gameObject.SetActive(false);
    }

    public void togglePause() {
        if(isPaused)
            Resume();
        else
            Pause();
        isPaused = !isPaused;
    }

    void Quit()
    {
        SceneManager.activeSceneChanged += Disconnect;
        SceneManager.LoadScene(0);
    }

    private void Disconnect(Scene prev, Scene next)
    {
        SceneManager.activeSceneChanged -= Disconnect;

        foreach (NetworkObject obj in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList)
        {
            Destroy(obj.gameObject);
        }

        gameObject.SetActive(false);
    }
}
