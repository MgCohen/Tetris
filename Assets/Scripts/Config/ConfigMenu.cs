using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMenu : MonoBehaviour
{
    public Config configFile;
    public void StartGame()
    {
        if (ViewController.instance.currentView != Views.Config) return;

        ViewController.instance.SetView(Views.Game, () => ViewController.instance.UnloadVIew(Views.Config));
    }

    public void UpdateX(string x)
    {
        var xValue = int.Parse(x);
        configFile.boardSize.x = xValue;
    }

    public void UpdateY(string y)
    {
        var yValue = int.Parse(y);
        configFile.boardSize.y = yValue;
    }

    public void UpdateSound(bool state)
    {
        configFile.soundEffects = state;
    }
}
