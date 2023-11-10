using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OnOffAudioPlay : MonoBehaviour
{
    [SerializeField]
    private AudioMixerGroup Mixer;
    
    public void Play_StopMusic()
    {

        Mixer.audioMixer.GetFloat("OnOffVolume", out float OnOffVolumeValue);
        if (OnOffVolumeValue < -79.0f)
        {
            Mixer.audioMixer.SetFloat("OnOffVolume", -5.0f);
        }
        else
        {
            Mixer.audioMixer.SetFloat("OnOffVolume", -80.0f);
        }
    }
}
