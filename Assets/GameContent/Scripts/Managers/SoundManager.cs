using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMB<SoundManager>
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioSource m_AudioSource2;


    public void PlaySFX(AudioClip clip)
    {
        if (m_AudioSource.isPlaying)
        {
            m_AudioSource.clip = clip;
            m_AudioSource.Play();

        }else
        {
            m_AudioSource2.clip = clip;
            m_AudioSource2.Play();
        }
    }
}
