using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void RestartGame()
    {
        if (ViewController.instance.currentView != Views.Pause) return;
        Controller.state = GameState.Starting;
        ViewController.instance.UnloadVIew(Views.Game);
        ViewController.instance.SetView(Views.Game, () => ViewController.instance.UnloadVIew(Views.Pause));
    }

    public void ResumeGame()
    {
        if (ViewController.instance.currentView != Views.Pause) return;
        ViewController.instance.SetView(Views.Game, () =>
        {
            Controller.state = GameState.Playing;
            ViewController.instance.UnloadVIew(Views.Pause);
        });
    }

    public void ExitGame()
    {
        if (ViewController.instance.currentView != Views.Pause) return;
        ViewController.instance.SetView(Views.Main, () =>
        {
            ViewController.instance.UnloadVIew(Views.Pause);
            ViewController.instance.UnloadVIew(Views.Game);
        });
    }
}
