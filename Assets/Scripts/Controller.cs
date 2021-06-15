using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Controller : MonoBehaviour
{
    public static GameState state;

    public Board board;
    public Piece currentPiece;
    public Pointer pointer;
    public float moveTime;
    float timer;
    public Preview preview;
    public static UnityEvent OnReady = new UnityEvent();
    int readyCount = 1;
    int readyCounter = 0;


    private void OnEnable()
    {
        OnReady.AddListener(CheckReady);
    }

    private void OnDisable()
    {
        OnReady.RemoveListener(CheckReady);
    }

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

    void HardDrop()
    {
        while (board.CheckMove(currentPiece))
        {
            currentPiece.Move(Vector3Int.down);
        }
        currentPiece.Move(Vector3Int.up);
        PlacePiece();
    }

    void SoftDrop()
    {
        currentPiece.Move(Vector3Int.down);
        if (!board.CheckMove(currentPiece))
        {
            currentPiece.Move(Vector3Int.up);
            Debug.Log(5);
            CheckBlockout();
            PlacePiece();
        }
    }

    void CheckBlockout()
    {
        foreach (Cube cube in currentPiece.cubes)
        {
            if (cube.transform.position.y < board.limits.y)
            {
                return;
            }
        }
        //blockout
        Debug.Log("Lose");
    }

    void SpawnPiece()
    {
        var piece = preview.NextPiece();
        piece.transform.SetParent(transform);
        piece.transform.localRotation = Quaternion.identity;
        piece.transform.position = new Vector3(board.limits.x / 2, board.limits.y + 2, 0);
        currentPiece = piece;
    }

    void PlacePiece()
    {
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
        }
        SpawnPiece();
    }

    public void Pause()
    {
        if (state != GameState.Playing) return;
        state = GameState.Paused;
        ViewController.instance.SetView(Views.Pause);
    }

    public void Unpause()
    {
        if (state != GameState.Paused) return;
    }

}

public enum GameState
{
    Starting = 0,
    Playing = 1,
    Paused = 2,
}