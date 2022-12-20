using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySpraySound(AudioClip clip)
    {
        if(_audioSource.isPlaying == false)
        {
            Debug.Log("Play");
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    public void StopSpraySound()
    {
        Debug.Log("Stop");
        _audioSource.clip = null;
        _audioSource.loop = false;
        _audioSource.Stop();
    }
}
