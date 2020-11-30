using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    enum Screen
    {
        None,
        Main,
        Settings,
    }

    public CanvasGroup mainScreen;
    public CanvasGroup settingsScreen;

    void SetCurrentScreen(Screen screen)
    {
        Utility.SetCanvasGroupEnabled(mainScreen, screen == Screen.Main);
        Utility.SetCanvasGroupEnabled(settingsScreen, screen == Screen.Settings);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void StartNewGame()
    {
        SetCurrentScreen(Screen.None);
        LoadingScreen.instance.LoadScene("SampleScene");
    }

    public void StartLevelOne()
    {
        StartLevel(1);
    }

    public void StartLevelTwo()
    {
        StartLevel(2);
    }

    private void StartLevel(int levelNumber)
    {
        switch (levelNumber) {
            case 1:
                SetCurrentScreen(Screen.None);
                LoadingScreen.instance.LoadScene("SampleScene");
                break;
            case 2:
                SetCurrentScreen(Screen.None);
                LoadingScreen.instance.LoadScene("SampleScene1");
                break;
            default:
                break;
        }
    }

    public void OpenSettings()
    {
        SetCurrentScreen(Screen.Settings);
    }

    public void CloseSettings()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
