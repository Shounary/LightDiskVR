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

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        resume.onClick.AddListener(Resume);
        quit.onClick.AddListener(Quit);
    }

    void Resume() {
        gameObject.SetActive(false);
    }

    void Quit()
    {
        SceneManager.activeSceneChanged += Disconnect;
        SceneManager.LoadScene(0);
    }

    private void Disconnect(Scene prev, Scene next)
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.activeSceneChanged -= Disconnect;
        gameObject.SetActive(false);
    }
}
