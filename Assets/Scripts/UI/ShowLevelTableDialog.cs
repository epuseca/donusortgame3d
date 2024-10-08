using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import UnityEngine.UI ?? s? d?ng Button

public class ShowLevelTableDialog : Dialog
{
    public int levelBtn; 
    private Button button; 
    public GameManager gm;
    public Sprite newSprite;
    public Sprite oldSprite;
    public Image levelImage;
    private int checkLevel;
    
    void Start()
    {
        button = GetComponent<Button>();

        if (gm && button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }
    private void Update()
    {
        if (gm && button != null)
        {
            CheckZoneAndLevelToChangeImage();
        }
    }
    private void CheckZoneAndLevelToChangeImage()
    {
        if (levelImage != null)
        {
            levelImage.sprite = oldSprite;
        }

        if (gm.GetZones() == 1)
        {
            checkLevel = Prefs.LevelDone1;
        }
        if (gm.GetZones() == 2)
        {
            checkLevel = Prefs.LevelDone2;
        }
        if (gm.GetZones() == 3)
        {
            checkLevel = Prefs.LevelDone3;
        }
        // Ki?m tra ?i?u ki?n levelBtn và thay ??i hình ?nh
        if (levelBtn < checkLevel)
        {
            levelImage.sprite = newSprite; // ??i ?nh c?a button có levelBtn <= Prefs.LevelDone1
        }

    }
    void OnButtonClick()
    {
        if (gm)
        {
            gm.SetLevelBtnGm(levelBtn);
        }
        Debug.Log("Zone " + gm.GetZones()); 

        Debug.Log("Level " + levelBtn); 
        if(gm.GetZones() == 1)
        {
            if(levelBtn <= Prefs.LevelDone1)
            {
                GameGUIManager.Ins.GameDialog("GameLevelTable", false);
                GameGUIManager.Ins.GameState("GameGui");
                //gm.PlayLevel1();
                gm.CheckPlayGame();
            }
            
        }
        if(gm.GetZones() == 2)
        {
            if (levelBtn <= Prefs.LevelDone2)
            {
                GameGUIManager.Ins.GameDialog("GameLevelTable", false);
                GameGUIManager.Ins.GameState("GameGui");
                //gm.PlayLevel2();
                gm.CheckPlayGame();

            }
        }
        if(gm.GetZones() == 3)
        {
            if (levelBtn <= Prefs.LevelDone3)
            {
                GameGUIManager.Ins.GameDialog("GameLevelTable", false);
                GameGUIManager.Ins.GameState("GameGui");
                //gm.PlayLevel3();
                gm.CheckPlayGame();

            }
        }
        
    }

    public override void Show(bool isShow)
    {
        base.Show(isShow);
    }

    
}
