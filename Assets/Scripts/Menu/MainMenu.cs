using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        if (ViewController.instance.currentView != Views.Main) return;

        ViewController.instance.SetView(Views.Config, () => ViewController.instance.UnloadVIew(Views.Main));
    }
}
