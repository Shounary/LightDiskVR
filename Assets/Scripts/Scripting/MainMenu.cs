using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public List<GameObject> menus = new List<GameObject>();
    public GameObject activeMenu;

    private void Start() {
        activeMenu = menus[0];
    }

    public void goToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void openSubMenu(int index)
    {
        if(menus[index].Equals(activeMenu))
            return;
        activeMenu.SetActive(false);
        menus[index].SetActive(true);
        activeMenu = menus[index];
    }

}
