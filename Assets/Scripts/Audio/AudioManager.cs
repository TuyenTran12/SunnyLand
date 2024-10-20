using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------------Audio Source--------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [Header("-------------Audio Clip--------------")]
    public AudioClip background;
    public AudioClip walk;
    public AudioClip jump;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip collectItem;
    public AudioClip EnemyDeath;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

