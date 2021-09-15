using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip sfxAxeHit;
    public AudioClip SfxSword;
    public AudioClip SfxDamagePlayer;
    public AudioClip SfxOpenChest;
    public AudioClip SfxForTheHorde;
    
    public AudioClip bgm;

    private AudioSource audioSource;

    public static AudioController current;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
}
