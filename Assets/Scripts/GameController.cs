using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public CanvasGroup buttonPanel;
    public Button button;
    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    Character currentTarget;
    bool waitingForInput;

    public Button restartLevelButton;
    public Button openMainMenuButton;
    public Button topLeftMenuButton;
    public CanvasGroup topLeftMenuContainer;

    public CanvasGroup resultBanner;
    public TMP_Text resultBannerText;
    public Button resultBannerRestartButton;
    public Button resultBannerMainMenuButton;


    Character FirstAliveCharacter(Character[] characters)
    {
        // LINQ: return enemyCharacter.FirstOrDefault(x => !x.IsDead());
        foreach (var character in characters)
        {
            if (!character.IsDead())
                return character;
        }
        return null;
    }
   
    void PlayerWon()
    {
        /*StopAllCoroutines();*/

        GameObject.Find("Canvas").GetComponent<CanvasGroup>().alpha = 1f;
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        Utility.SetCanvasGroupEnabled(topLeftMenuContainer, false);
        

        resultBanner.alpha = 1f;
        resultBannerText.text = "Victory";
        
    }

    void PlayerLost()
    {
        /*StopAllCoroutines();*/

        GameObject.Find("Canvas").GetComponent<CanvasGroup>().alpha = 1f;
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        Utility.SetCanvasGroupEnabled(topLeftMenuContainer, false);
        

        resultBanner.alpha = 1f;
        resultBannerText.text = "Defeat";
        
    }

    bool CheckEndGame()
    {
        if (FirstAliveCharacter(playerCharacter) == null)
        {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacter) == null)
        {
            PlayerWon();
            return true;
        }

        return false;
    }

    void PlayerAttack()
    {
        waitingForInput = false;
    }

    public void NextTarget()
    {
        int index = Array.IndexOf(enemyCharacter, currentTarget);
        for (int i = 1; i < enemyCharacter.Length; i++)
        {
            int next = (index + i) % enemyCharacter.Length;
            if (!enemyCharacter[next].IsDead())
            {
                currentTarget.targetIndicator.gameObject.SetActive(false);
                currentTarget = enemyCharacter[next];
                currentTarget.targetIndicator.gameObject.SetActive(true);
                return;
            }
        }
    }

    IEnumerator GameLoop()
    {
        yield return null;
        while (!CheckEndGame())
        {
            foreach (var player in playerCharacter)
            {
                if (!player.IsDead())
                {
                    currentTarget = FirstAliveCharacter(enemyCharacter);
                    if (currentTarget == null)
                        break;

                    currentTarget.targetIndicator.gameObject.SetActive(true);
                    Utility.SetCanvasGroupEnabled(buttonPanel, true);

                    waitingForInput = true;
                    while (waitingForInput)
                        yield return null;

                    Utility.SetCanvasGroupEnabled(buttonPanel, false);
                    currentTarget.targetIndicator.gameObject.SetActive(false);

                    player.target = currentTarget.transform;
                    player.AttackEnemy();

                    while (!player.IsIdle())
                        yield return null;

                    break;
                }
            }

            foreach (var enemy in enemyCharacter)
            {
                if (!enemy.IsDead())
                {
                    Character target = FirstAliveCharacter(playerCharacter);
                    if (target == null)
                        break;

                    enemy.target = target.transform;
                    enemy.AttackEnemy();

                    while (!enemy.IsIdle())
                        yield return null;

                    break;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(PlayerAttack);

        initSceneCharacters();

        Utility.SetCanvasGroupEnabled(buttonPanel, true);
        resultBanner.alpha = 0.0f;

        StartCoroutine(GameLoop());
    }

    private void initSceneCharacters()
    {
        var charactersContainer = GameObject.Find("Characters").transform;
        if (charactersContainer != null)
        {
            var charactersCountInScene = charactersContainer.childCount;
            if (charactersCountInScene > 0)
            {
                for (int index = 0; index < charactersCountInScene; index++)
                {
                    var character = charactersContainer.GetChild(index);

                    if (character.name.Contains("Hooligan"))
                    {
                        playerCharacterList.Add(character.GetComponent<Character>());
                    }

                    if (character.name.Contains("Policeman"))
                    {
                        computerCharacterList.Add(character.GetComponent<Character>());
                    }
                }
            }
        }
    }

    public List<Character> computerCharacterList = new List<Character>();
    public List<Character> playerCharacterList = new List<Character>();

    public void OnTopLeftButtonClicked()
    {
        var isTopLefContainerVisible = topLeftMenuContainer.alpha > 0;
        if (isTopLefContainerVisible)
        {
            topLeftMenuButton.GetComponentInChildren<TMP_Text>().text = "Pause";
            Utility.SetCanvasGroupEnabled(topLeftMenuContainer, false);
        }
        else
        {
            topLeftMenuButton.GetComponentInChildren<TMP_Text>().text = "Resume";
            Utility.SetCanvasGroupEnabled(topLeftMenuContainer, true);
        }
    }

    public void RestartLevel()
    {
        LoadingScreen.instance.LoadScene("SampleScene");
    }

    public void OpenMainMenu()
    {
        LoadingScreen.instance.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
