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
                _main.clips = Resources.Load<SoundClips>("Sounds");
                _main.configFile = Resources.Load<Config>("Configs");
            }
            DontDestroyOnLoad(_main);
            return _main;
        }
    }
    static SoundBox _main;

    public AudioSource audioSource;
    public Config configFile;
    public SoundClips clips;

    public static void DropSound()
    {
        if (!main.configFile.soundEffects) return; 
        main.audioSource.clip = main.clips.Clip("Drop");
        main.audioSource.Play();
    }

    public static void ScoreSound()
    {
        if (!main.configFile.soundEffects) return;
        main.audioSource.clip = main.clips.Clip("Score");
        main.audioSource.Play();
    }

    public static void GameOver()
    {
        if (!main.configFile.soundEffects) return;
        main.audioSource.clip = main.clips.Clip("GameOver");
        main.audioSource.Play();
    }

}
