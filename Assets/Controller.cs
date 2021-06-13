using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Board board;

    public Piece currentPiece;

    public float moveTimer;
    float timeCounter;

    public Piece examplePiece;

    private void Update()
    {
        MovementInput();
        RotationInput();

        timeCounter += Time.deltaTime;
        if (timeCounter >= moveTimer)
        {
            timeCounter = 0;
            DropPiece();
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
            PlacePiece();
        }
    }

    void RotationInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentPiece.Rotate(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentPiece.Rotate(-1);
        }
    }

    void DropPiece()
    {
        currentPiece.Move(Vector3Int.down);
        if (!board.CheckMove(currentPiece))
        {
            currentPiece.Move(Vector3Int.up);
            Debug.Log(5);
            PlacePiece();
            foreach (Cube cube in currentPiece.cubes)
            {
                if (cube.transform.position.y < board.limits.y)
                {
                    return;
                }
            }
            //blockout
        }
    }

    void SpawnPiece(Piece piece)
    {
        currentPiece = Instantiate(piece, new Vector3(5, 22, 0), Quaternion.identity);
    }

    void PlacePiece()
    {
        board.PlacePiece(currentPiece);
        SpawnPiece(examplePiece);
        timeCounter = 0;
    }
}
