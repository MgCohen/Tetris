using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Configs", menuName = "Config/Options")]
public class Config : ScriptableObject
{
    [BoxGroup("Game Settings")]
    public Vector2Int boardSize;

    [BoxGroup("Sound Settings")]
    [OnValueChanged("ChangeSound")]
    public bool soundEffects;

    [HideInInspector]
    public UnityEvent<bool> OnSoundChange = new UnityEvent<bool>();


    void ChangeSound()
    {
        OnSoundChange.Invoke(soundEffects);
    }

}
