using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Main Controller of game
public class Board : MonoBehaviour
{
    public Vector2 limits = new Vector2(10, 20);
    public Cube[,] boardFill;

    public GameObject backgroundCube;

    private void Start()
    {
        boardFill = new Cube[(int)limits.x, (int)limits.y + 4];
        for (int i = 0; i < limits.x; i++)
        {
            for (int j = 0; j < limits.y; j++)
            {
                Instantiate(backgroundCube, new Vector3(i, j, -1), Quaternion.identity);
            }
        }
    }

    public bool CheckMove(Piece piece)
    {
        foreach (Cube cube in piece.cubes)
        {
            Vector2 cubePos = cube.transform.position;
            Vector2 wantedPos = new Vector2(cubePos.x, cubePos.y );

            //check if cube is inside valid area
            if (wantedPos.x < 0 || wantedPos.x >= limits.x || wantedPos.y < 0)
            {
                Debug.Log(1);
                return false;
            }

            //check if there is anything on the target space
            if (boardFill[(int)wantedPos.x, (int)wantedPos.y] != null)
            {
                Debug.Log(2);
                return false;
            }

        }
        return true;
    }

    public void PlacePiece(Piece piece)
    {
        foreach(Cube cube in piece.cubes)
        {
            Vector2 piecePos = cube.transform.position;
            Vector2Int movePos = new Vector2Int((int)piecePos.x, (int)piecePos.y);

            boardFill[movePos.x, movePos.y] = cube;
        }
    }

/// <summary>
/// Check every space on that line, if all are true returns true as a line is filled.
/// </summary>
    public bool CheckLine(int height)
    {
        for (int i = 0; i < limits.x; i++)
        {
            if (boardFill[i, height] == null) return false;
        }

        return true;
    }
}
