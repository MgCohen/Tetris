using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ViewController : MonoBehaviour
{
    public static ViewController instance;

    public Views currentView;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            SetView(Views.Main);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetView(Views view, UnityAction callback = null)
    {
        currentView = view;
        if (view == Views.Main)
        {
            transform.DORotate(new Vector3(0, 180, 0), 0.8f).OnComplete(callback.Invoke);
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
        else if (view == Views.Config)
        {
            transform.DORotate(new Vector3(0, 90, 0), 0.8f).OnComplete(callback.Invoke);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
        else if (view == Views.Game)
        {
            transform.DORotate(new Vector3(0, 0, 0), 0.8f).OnComplete(callback.Invoke);
            if (Controller.state == GameState.Starting)
            {
                SceneManager.LoadScene(3, LoadSceneMode.Additive);
            }
        }
        else if (view == Views.Pause)
        {
            transform.DORotate(new Vector3(0, 270, 0), 0.8f).OnComplete(callback.Invoke);
            SceneManager.LoadScene(4, LoadSceneMode.Additive);
        }
    }

    public void UnloadVIew(Views view)
    {
        SceneManager.UnloadSceneAsync((int)view);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A)) SetView(Views.Game);
    //}
}

public enum Views
{
    Main = 1,
    Config = 2,
    Game = 3,
    Pause = 4,
}
