using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : AUIPage {

    
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle showHelpToggle;

    public void OnMusicSliderChange(float value) {
        PlayerPrefs.SetFloat("MusicVol", value);
        AudioManager.instance.SetVolume(false);
        //AppManager.instance.preferences.musicVol = value;
    }

    public void OnSfxSliderChange(float value) {
        PlayerPrefs.SetFloat("SFXVol", value);
        AudioManager.instance.SetVolume(true);
        //AppManager.instance.preferences.sfxVol = value;
    }

    public void OnHelpMenuToggleChange(bool value) {
        PlayerPrefs.SetInt("ShowHelp", value ? 1 : 0);
        //AppManager.instance.preferences.showHelp = value;
    }

    public void OpenCredits() {
        uiController.Open(3);
    }

    public void Back () {
        uiController.Open(0);
    }

    public override void OnOpen() {
        //GamePrefs preferences = AppManager.instance.preferences;
        //musicSlider.value = preferences.musicVol;
        //sfxSlider.value = preferences.sfxVol;
        //showHelpToggle.isOn = preferences.showHelp;
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
        showHelpToggle.isOn = PlayerPrefs.GetInt("ShowHelp") != 0;
    }
}
