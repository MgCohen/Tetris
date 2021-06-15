using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Config/Sounds")]
public class SoundClips : ScriptableObject
{
    [SerializeField]
    public List<Clips> sounds = new List<Clips>();

    public AudioClip Clip(string id)
    {
        return sounds.Find(x => x.id == id);
    }
}

[System.Serializable]
public class Clips
{
    public string id;
    public AudioClip sound;

    public static implicit operator AudioClip(Clips clip) => clip.sound;
}
