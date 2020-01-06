﻿using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    //[SerializeField] string menuTheme;
    [Space]
    [SerializeField] SoundClip[] sounds = new SoundClip[0];
    SoundClip currentTrack = null;

    public float mastervolume;
    public float musicvolume;
    public float sfxvolume;
    
    public void Start()
    {
        currentTrack = sounds[0];
        foreach (SoundClip s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.isLooping;
        }
    }

    public void Play(string clipToPlay)
    {
        
        print(" I'm now playing - " + clipToPlay);

        if (currentTrack.isMusic)
        {
            currentTrack.source.Stop();
        }

        SoundClip s = Array.Find(sounds, sounds => sounds.name == clipToPlay);
        currentTrack = s;

        if (currentTrack.isMusic)
        {
            s.source.volume = s.volume * mastervolume * musicvolume;
        }
        else
        {
            s.source.volume = s.volume * mastervolume * sfxvolume;
        }
        if(!s == null)
        {
            s.source.Play();
        }        
    }
}

[Serializable]
public class SoundClip
{
    public string name;
    public bool isMusic;
    public bool isLooping;
    public AudioClip clip;

    [HideInInspector] public AudioSource source;

    [Range(0, 1)] public float volume = 1;

    [Range(0, 3)] public float pitch = 1;

}