using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{
    static SoundBox main
    {
        get
        {
            if (_main == null)
            {
                GameObject box = new GameObject("SoundBox");
                _main = box.AddComponent<SoundBox>();
                _main.audioSource = box.AddComponent<AudioSource>();
            }
            DontDestroyOnLoad(_main);
            return _main;
        }
    }
    static SoundBox _main;

    public AudioSource audioSource;
    public SoundClips clips
    {
        get
        {
            if (_clips == null) _clips = Resources.Load<SoundClips>("Sounds");
            return _clips;
        }
    }
    SoundClips _clips;

    public static void DropSound()
    {
        main.audioSource.clip = main.clips.Clip("Drop");
        Debug.Log("a");
        main.audioSource.Play();
    }

    public static void ScoreSound()
    {
        main.audioSource.clip = main.clips.Clip("Score");
        main.audioSource.Play();
    }

    public static void GameOver()
    {
        main.audioSource.clip = main.clips.Clip("GameOver");
        Debug.Log("c");
        main.audioSource.Play();
    }

}
