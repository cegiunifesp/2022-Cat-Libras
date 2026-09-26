using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource _audio;

    static float volume;

    // variaveis estaticas
    public float Volume
    {
        get { return volume; }
        set { volume = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.volume = Volume;
    }

    public void ChangeVolume(float value)
    {
        _audio.volume = value;
        Volume = value;
    }
}
