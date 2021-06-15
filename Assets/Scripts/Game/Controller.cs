using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Controller : MonoBehaviour
{
    public static GameState state;

    public static UnityEvent OnReady = new UnityEvent();

    public Board board;
    public Piece currentPiece;
    public Pointer pointer;
    public Preview preview;
    public GameObject GameoverPanel;
    public float moveTime;
    float timer;
    int readyCount = 1;
    int readyCounter = 0;



    private void OnEnable()
    {
        state = GameState.Starting;
        OnReady.AddListener(CheckReady);
    }

    private void OnDisable()
    {
        OnReady.RemoveListener(CheckReady);
    }

    /// <summary>
    /// Basic Check if all events are ready
    /// </summary>
    void CheckReady()
    {
        readyCounter++;
        if (readyCounter >= readyCount) { SpawnPiece(); state = GameState.Playing; }
    }

    private void Update()
    {
        if (state != GameState.Playing) return;

        MovementInput();
        RotationInput();

        timer += Time.deltaTime;
        if (timer >= moveTime)
        {
            timer = 0;
            SoftDrop();
        }
    }

    /// <summary>
    /// controls all movement inputs
    /// </summary>
    void MovementInput()
    {
        if (Input.GetKeyDown(KeyCode.A)) //RIGHT
        {
            currentPiece.Move(Vector3Int.right);
            if (!board.CheckMove(currentPiece))
                currentPiece.Move(Vector3Int.left);
        }
        else
        if (Input.GetKeyDown(KeyCode.D)) //LEFT
        {
            currentPiece.Move(Vector3Int.left);
            if (!board.CheckMove(currentPiece))
                currentPiece.Move(Vector3Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentPiece.Move(Vector3Int.down);
            if (!board.CheckMove(currentPiece))
                currentPiece.Move(Vector3Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) //DROP
        {
            HardDrop();
        }
    }

    /// <summary>
    /// controls all rotation inputs
    /// </summary>
    void RotationInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentPiece.Rotate(1);
            if (!board.CheckMove(currentPiece))
                currentPiece.Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentPiece.Rotate(-1);
            if (!board.CheckMove(currentPiece))
                currentPiece.Rotate(1);
        }
    }

    /// <summary>
    /// forcefully drops the piece until its final position and places it
    /// </summary>
    void HardDrop()
    {
        while (board.CheckMove(currentPiece))
        {
            currentPiece.Move(Vector3Int.down);
        }
        currentPiece.Move(Vector3Int.up);
        PlacePiece();
        if (CheckBlockout()) GameOver();
        else SpawnPiece();
    }

    /// <summary>
    /// Lower the piece by 1 space and verifies if it can continue
    /// </summary>
    void SoftDrop()
    {
        currentPiece.Move(Vector3Int.down);
        if (!board.CheckMove(currentPiece))
        {
            currentPiece.Move(Vector3Int.up);
            PlacePiece();
            if (CheckBlockout()) GameOver();
            else SpawnPiece();
        }
    }

    /// <summary>
    /// check if the piece has at least one cube inside a valid playarea, gameover if you dont
    /// </summary>
    bool CheckBlockout()
    {
        foreach (Cube cube in currentPiece.cubes)
        {
            if (cube.transform.position.y < board.limits.y)
            {
                return false;
            }
        }
        //blockout
        return true;

    }

    /// <summary>
    /// Calls for a new piece
    /// </summary>
    void SpawnPiece()
    {
        var piece = preview.NextPiece();
        piece.transform.SetParent(transform);
        piece.transform.localRotation = Quaternion.identity;
        piece.transform.position = new Vector3(board.limits.x / 2, board.limits.y + 2, 0);
        currentPiece = piece;
    }

    /// <summary>
    /// Fix the piece on its current position
    /// </summary>
    void PlacePiece()
    {
        SoundBox.DropSound();
        board.PlacePiece(currentPiece);
        timer = 0;
        var heights = currentPiece.cubes.Select(x => Mathf.RoundToInt(x.transform.position.y)).Distinct();
        int scoredLines = 0;
        foreach (var h in heights)
        {
            if (board.CheckLine(h)) scoredLines++;
        }
        if(scoredLines > 0)
        {
            board.ActivateGravity(heights.OrderBy(x => x).First(), scoredLines);
            pointer.Score(scoredLines);
            SoundBox.ScoreSound();
        }
    }

    /// <summary>
    /// pauses the game
    /// </summary>
    public void Pause()
    {
        if (state != GameState.Playing) return;
        state = GameState.Paused;
        ViewController.instance.SetView(Views.Pause);
    }

    /// <summary>
    /// triggers the end game
    /// </summary>
    public void GameOver()
    {
        SoundBox.GameOver();
        state = GameState.Paused;
        Debug.Log("Lose");
        GameoverPanel.SetActive(true);
    }

    /// <summary>
    /// restarts the game state and scenes
    /// </summary>
    public void Exit()
    {
        GameoverPanel.SetActive(false);
        state = GameState.Exiting;
        ViewController.instance.SetView(Views.Main, () => ViewController.instance.UnloadVIew(Views.Game));
    }

}

public enum GameState
{
    Starting = 0,
    Playing = 1,
    Paused = 2,
    Exiting = 3,
}