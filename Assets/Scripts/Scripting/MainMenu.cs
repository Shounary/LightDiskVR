using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //0 = home screen  1 - play SM 2 - controls SM 3 - settings SM 4 - quit
    //1 = play screen  1 - tutorial SM 2 - single player SM 3 - multiplayer SC 4 - back
    //2 = tutorial screen 1 - basic SC 2 - weapons SC 3 - movement SC 4 - back
    //3 = 1p screen 1 - sandbox SC 2 - survival SC 3 - shooting range SC 4 - back
    //4 = controls
    //5 = settings
    public List<GameObject> menus = new List<GameObject>();
    public GameObject activeMenu;

    int currActiveMenu;
    int prevActiveMenu;

    private void Start() {
        activeMenu = menus[0];
    }

    public void onButton1Click() {
        switch(currActiveMenu) {
            case 0:
                openSubMenu(1);
                break;
            case 1:
                openSubMenu(2);
                break;
            case 2:
                goToScene("Basic Tutorial");
                break;
            case 3:
                goToScene("Single Player");
                break;
            default:
                break;
        }

    }

    public void onButton2Click() {
        switch(currActiveMenu) {
            case 0:
                //openSubMenu(4);
                break;
            case 1:
                openSubMenu(3);
                break;
            case 2:
                goToScene("Weapon Tutorial");
                break;
            case 3:
                goToScene("Survival Mode");
                break;
            default:
                break;
        }
    }

    public void onButton3Click() {
        switch(currActiveMenu) {
            case 0:
                break;
            case 1:
                goToScene("Lobby");
                break;
            case 2:
                goToScene("Adv Tutorial");
                break;
            case 3:
                goToScene("ShootingRange70sec");
                break;
            default:
                break;
        }
    }

    public void onButton4Click() {
        switch(currActiveMenu) {
            case 0:
                Quit();
                break;
            default:
                openSubMenu(prevActiveMenu);
                break;
        }
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
        prevActiveMenu = currActiveMenu;
        currActiveMenu = index;
    }

}
