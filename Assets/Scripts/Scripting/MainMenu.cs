using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //0 = home screen  1 - play 2 - controls 3 - settings 4 - quit
    //1 = play screen  1 - multiplayer 2 - single player 3 - tutorial 4 - back
    //2 = tutorial screen 1 - basic 2 - weapons 3 - movement 4 - back
    //3 = 1p screen 1 - shooting range 2 - survival 3 - sandbox 4 - back
    public List<GameObject> menus = new List<GameObject>();
    public GameObject activeMenu;

    int currActiveMenu;
    int prevActiveMenu;

    private void Start() {
        activeMenu = menus[0];
    }

    public void onButton1Click() {

    }

    public void onButton2Click() {

    }

    public void onButton3Click() {

    }

    public void onButton4Click() {

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
