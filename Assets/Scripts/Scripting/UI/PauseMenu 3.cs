using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
<<<<<<< Updated upstream
using FMODUnity;
=======
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
    private void OnEnable()
    {
        RuntimeManager.PlayOneShot("event:/UI/Simple_Hover", transform.position);
    }

=======
>>>>>>> Stashed changes
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
