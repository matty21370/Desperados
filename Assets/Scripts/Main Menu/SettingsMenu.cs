using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public AudioMixer masterMixer;
    public AudioMixer sfxMixer;
    public AudioMixer musicMixer;

    public Toggle fullscreenToggle;

    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider masterSlider;

    

    // Start is called before the first frame update
    void Start()
    {
        //initialise array with all possible resolutions
        resolutions = Screen.resolutions;

        //ensure there's nothing already in the dropdown
        resolutionDropdown.ClearOptions();

        //create a list of strings that will hold the options for resolution
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        //creating the string containing the width & height for each resolution
        //then adding to list
        for (int i = 0; i < resolutions.Length; i ++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);


            //finding the current resolution of window
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height) 
            {
                currentResolutionIndex = i;
            }
        }

        //updating the dropdown
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        /* ensuring the toggle is checked or unchecked
        depending on whether fullscreen is already enabled
        when opening settings menu at the start */
        if (Screen.fullScreen)
        {
            fullscreenToggle.isOn = true;
        } else
        {
            fullscreenToggle.isOn = false;
        }

        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }

        GetComponent<CanvasGroup>().alpha = 1;
        gameObject.SetActive(false);
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void setMasterVolume(float volume)
    {
        masterMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void setSFXVolume(float volume)
    {
        sfxMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void setMusicVolume(float volume)
    {
        musicMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

}
