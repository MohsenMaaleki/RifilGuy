using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void Setvolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
