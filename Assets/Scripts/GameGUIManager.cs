using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGUIManager : Singleton<GameGUIManager>
{
    public GameObject homeGui;
    //public GameObject gameGui;
    public Dialog gameGui;

    public Dialog gameSettingDialog;
    public Dialog winLevelDialog;
    public Dialog loseLevelDialog;
    public Dialog tutorialDialog;
    public Dialog gamelevelDialog;
    public Dialog gametablelevelDialog;
    public Text coins_bonus_text;
    public Text diamond_remaining;
    public Text coins_remaining_lose;
    public override void Awake()
    {
        MakeSingleton(false);
    }
    public void ShowGameGUI(bool isShow)
    {
        if (gameGui)
            gameGui.Show(isShow);
        if (homeGui)
            homeGui.SetActive(!isShow);
    }
    public void HideGameGui()
    {
        if(homeGui)
            homeGui.SetActive(false);
        if(gameGui)
            gameGui.Show(false);
    }
    public void ShowGameHome(bool isShow)
    {
        if (homeGui)
            homeGui.SetActive(isShow);
    }
    public void ShowGameGui(bool isShow)
    {
        if (homeGui)
            gameGui.Show(isShow);
    }
    public void ShowTutorialGui(bool isShow)
    {
        tutorialDialog.Show(isShow);
    }
    public void ShowGameSettingDialog(bool isShow)
    {
        gameSettingDialog.Show(isShow);
    }
    public void ShowWinLevelDialog(bool isShow)
    {
        winLevelDialog.Show(isShow);
    }
    public void ShowLoseLevelDialog(bool isShow)
    {
        loseLevelDialog.Show(isShow);
    }
    public void ShowLevelDialog(bool isShow)
    {
        gamelevelDialog.Show(isShow);
    }
    public void ShowTableLevelDialog(bool isShow)
    {
        gametablelevelDialog.Show(isShow);
    }
    public void GameState(string state)
    {
        switch (state)
        {
            case "GameHome":
                homeGui.SetActive(true);
                gamelevelDialog.Show(false);
                gameGui.Show(false);
                break;
            case "GameGui":
                homeGui.SetActive(false);
                gamelevelDialog.Show(false);
                gameGui.Show(true);
                break;
            case "GameLevel":
                homeGui.SetActive(false);
                gamelevelDialog.Show(true);
                gameGui.Show(false);
                break;
        }
    }
    public void GameDialog(string state, bool isbool)
    {
        switch (state)
        {
            case "GameSetting":
                gameSettingDialog.Show(isbool);
                break;
            case "GameWin":
                winLevelDialog.Show(isbool);
                break;
            case "GameLose":
                loseLevelDialog.Show(isbool);
                break;
            case "Tutorial":
                tutorialDialog.Show(isbool);
                break;
            case "GameLevelTable":
                gametablelevelDialog.Show(isbool);
                break ;
        }
    }
    public void UpdateCoins()
    {
        if (diamond_remaining)
        {
            diamond_remaining.text = Prefs.Coins.ToString();
        }
    }
    public void SetTextCoinsBonus(int coins)
    {
        if (coins_bonus_text)
        {
            coins_bonus_text.text = coins.ToString();
        }
    }
    public void CoinsBonus(int bonus_coins)
    {
        if (coins_bonus_text)
        {
            Prefs.Coins += bonus_coins;
        }
    }
    public void CoinsUsed(int coins)
    {
        Prefs.Coins -= coins;
    }
    public void CoinsRemainLose()
    {
        if (coins_remaining_lose)
        {
            coins_remaining_lose.text = "Remaining Coins: " + Prefs.Coins.ToString();
        }
    }
}
