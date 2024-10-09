using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();  // Start playing the music
    }

    public void StopMusic()
    {
        audioSource.Stop();  // Stops the music
    }

    public void PauseMusic()
    {
        audioSource.Pause();  // Pauses the music
    }

    public void ResumeMusic()
    {
        audioSource.UnPause();  // Resumes the music
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;  // Adjusts the volume
    }
}
