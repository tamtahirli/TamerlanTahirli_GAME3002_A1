using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource source;
    public AudioClip goal, kick;
    public static Sound Instance;
    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audio)
    {
        source.clip = audio;
        source.Play();
    }
}
