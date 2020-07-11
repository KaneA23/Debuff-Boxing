using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
    Author:     Chris Lamb
    Last edit:  17/02/2019
    Purpose:    Logic for the settings menu. Needs to be completed
*/

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;


    // Volume Function
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    // Quality changes Function
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
