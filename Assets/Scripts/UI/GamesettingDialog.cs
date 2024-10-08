using UnityEngine;
using UnityEngine.UI;

public class GamesettingDialog : Dialog
{
    AudioController ac;

    [Header("UI Elements")]
    //public Button MusicSwitchBtn; // Reference to the button
    public Sprite switchOn; // Image for when music is playing
    public Sprite switchOff; // Image for when music is not playing
    public Image currentBtn;
    public Image currentSFXBtn;
    void Start()
    {
        //switchOff.GetComponent<Image>().sprite = currentBtn;
        ac = AudioController.Ins; // Assuming you are using the Singleton pattern for AudioController
        UpdateMusicSwitchBtnImage(); // Set the initial state of the button
        currentBtn.sprite = switchOn;
        currentSFXBtn.sprite = switchOn;
    }

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        UpdateMusicSwitchBtnImage(); // Update the button image when the dialog is shown
    }

    public void OnMusicSwitchBtnClick()
    {
        if (ac)
        {
            // Toggle music playing state (example)
            if (ac.IsPlayingMusic())
            {
                ac.StopPlayMusic();
            }
            else
            {
                ac.PlayBackgroundMusic();
            }
            UpdateMusicSwitchBtnImage(); // Update the button image after toggling
        }
    }

    private void UpdateMusicSwitchBtnImage()
    {
        if (ac)
        {
            if (ac.IsPlayingMusic())
            {
                currentBtn.sprite = switchOn;
            }
            else
            {
                currentBtn.sprite = switchOff;
            }
        }
    }
    public void OnSFXSwitchBtnClick()
    {
        if (ac)
        {
            // Toggle music playing state (example)
            if (ac.IsStateSfx())
            {
                ac.StopPlaySfx();
            }
            else
            {
                ac.StopPlaySfx();
            }
            UpdateSFXSwitchBtnImage(); // Update the button image after toggling
        }
    }

    private void UpdateSFXSwitchBtnImage()
    {
        if (ac)
        {
            if (ac.IsStateSfx())
            {
                currentSFXBtn.sprite = switchOn;
            }
            else
            {
                currentSFXBtn.sprite = switchOff;
            }
        }
    }
}
