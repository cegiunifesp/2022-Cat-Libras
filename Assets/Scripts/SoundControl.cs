using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public static bool muteSound { get; set; }

    public bool firstTime = true;

    [Header("Musica")]
    public float volumeMusic;
    public AudioSource music;
    [Header("SFX")]
    public float volumeSfx;
    public AudioSource sfx;
    [Header("Components")]
    public Toggle _toggle;

    // Start is called before the first frame update
    void Start()
    {
        if (!muteSound)
        {
            _toggle.isOn = true;
            music.volume = volumeMusic;
            sfx.volume = volumeSfx;
        }
        else
        {
            _toggle.isOn = false;
            music.volume = 0.0f;
            sfx.volume = 0.0f;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeStatus()
    {
        if (!_toggle.isOn)
        {
            music.volume = 0.0f;
            sfx.volume = 0.0f;
            muteSound = true;
        }
        else
        {
            music.volume = volumeMusic;
            sfx.volume = volumeSfx;
            muteSound = false;
        }
    }
}
