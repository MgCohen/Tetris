using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Controller : MonoBehaviour
{
    public Board board;
    public Piece currentPiece;

    public float moveTime;
    float timer;

    public DrawBag bag;

    bool started = false;

    public static UnityEvent OnReady = new UnityEvent();
    int readyCount = 1;
    int counter = 0;

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
        counter++;
        Debug.Log(1);
        if (counter >= readyCount) { SpawnPiece(); started = true; }
    }

    private void Update()
    {
        if (!started) return;

        MovementInput();
        RotationInput();

        timer += Time.deltaTime;
        if (timer >= moveTime)
        {
            timer = 0;
            MoveDown();
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
        else if (Input.GetKeyDown(KeyCode.Space)) //DROP
        {
            DropPiece();
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

    void DropPiece()
    {
        while (board.CheckMove(currentPiece))
        {
            currentPiece.Move(Vector3Int.down);
        }
        currentPiece.Move(Vector3Int.up);
    }

    void MoveDown()
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
        var piece = bag.GetPiece();
        currentPiece = Instantiate(piece, new Vector3(5, 23, 0), Quaternion.identity);
    }

    void PlacePiece()
    {
        board.PlacePiece(currentPiece);
        SpawnPiece();
        timer = 0;
        board.CheckLines();
    }
}
