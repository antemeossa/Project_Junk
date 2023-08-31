using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> soundtracks = new List<AudioClip>();
    private AudioSource audioSource;
    public AudioSource btnSource;
    private int currentSongIndex = 0;

    [SerializeField]
    private AudioClip mechanicClick, synthClick, lvlUp;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySong(currentSongIndex);
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            currentSongIndex = (currentSongIndex + 1) % soundtracks.Count;
            PlaySong(currentSongIndex);
        }
    }

    private void PlaySong(int index)
    {
        audioSource.clip = soundtracks[index];
        audioSource.Play();
    }

    public void playBtnSound()
    {
        btnSource.volume = .8f;
        btnSource.clip = mechanicClick;
        btnSource.Play();
    }

    public void playDeclineSound()
    {
        btnSource.volume = .5f;
        btnSource.clip = synthClick;
        btnSource.Play();
    }

    public void playLevelUpSound()
    {
        btnSource.volume = .5f;
        btnSource.clip = lvlUp;
        btnSource.Play();
    }
}
