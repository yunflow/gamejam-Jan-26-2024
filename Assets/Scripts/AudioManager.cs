using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class AudioDict
{
    public string name;
    public AudioClip audio;
}

public class AudioManager : MonoBehaviour
{
    public AudioSource tutorSpeech;

    private AudioSource audioSource;

    public AudioDict[] audioDicts;

    public void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(this.gameObject);
    }

    public AudioClip GetAudioClip(string name)
    {

        foreach (var item in audioDicts)
        {
            if (item.name == name)
            {
                return item.audio;
            }
        }

        throw new Exception("Cannot find the audio clip named: " + name);
    }

    public void PlaySound(string name)
    {
        audioSource.PlayOneShot(GetAudioClip(name));
    }

    public void PlaySoundRandom(params string[] names)
    {
        if (names.Length == 0)
        {
            throw new Exception("Parameter cannot be empty.");
        }

        int random = Random.Range(0, names.Length);

        audioSource.PlayOneShot(GetAudioClip(names[random]));
        Debug.Log("Playing: " + names[random]);
    }

    public void PauseSound() {
        audioSource.Pause();
    }

    public void PlayTutorSpeech() {
        tutorSpeech.Play();
    }
    public void PauseTutorSpeech() {
        tutorSpeech.Pause();
    }

}
