using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotkey : MonoBehaviour
{
    public Button button;

    public KeyCode key;

    private void Update()
    {
        if (button.interactable && Input.GetKeyDown(key))
        {
            button.onClick.Invoke();
        }
    }
}
