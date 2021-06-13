using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Main Controller of game
public class Board : MonoBehaviour
{
    public Vector2 limits = new Vector2(10, 20);
    public Cube[,] boardFill;
    public PopupCube[,] bgFill;
    public PopupCube bgCube;

    private void Start()
    {
        boardFill = new Cube[(int)limits.x, (int)limits.y + 4];
        bgFill = new PopupCube[(int)limits.x, (int)limits.y];
        StartCoroutine(BackgroundCO());
    }

    public bool CheckMove(Piece piece)
    {
        foreach (Cube cube in piece.cubes)
        {
            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(cube.transform.position.x), Mathf.RoundToInt(cube.transform.position.y));

            //check if cube is inside valid area
            if (pos.x < 0 || pos.x >= limits.x || pos.y < 0)
            {
                Debug.Log(1);
                return false;
            }

            //check if there is anything on the target space
            if (boardFill[(int)pos.x, (int)pos.y] != null)
            {
                Debug.Log(2);
                return false;
            }

        }
        return true;
    }

    public void PlacePiece(Piece piece)
    {
        foreach (Cube cube in piece.cubes)
        {
            Vector2 piecePos = cube.transform.position;
            Vector2Int movePos = new Vector2Int(Mathf.RoundToInt(piecePos.x), Mathf.RoundToInt(piecePos.y));

            boardFill[movePos.x, movePos.y] = cube;
            bgFill[movePos.x, movePos.y].Mark();
        }
    }

    /// <summary>
    /// Check every space on that line, if all are true returns true as a line is filled.
    /// </summary>
    public bool CheckLines()
    {
        for (int i = 0; i < limits.x; i++)
        {
            if (boardFill[i, 0] == null) return false;
        }

        return true;
    }

    IEnumerator BackgroundCO()
    {
        Vector3 center = new Vector3(limits.x / 2, 0);
        int counter = 1;
        bool hasNew = true;
        while (hasNew)
        {
            hasNew = false;
            for (int i = -counter; i <= counter; i++)
            {
                for (int j = -counter; j <= counter; j++)
                {
                    Vector3 Pos = new Vector3(i, j, -1) + center;
                    if (Pos.x >= 0 && Pos.x < limits.x && Pos.y >= 0 && Pos.y < limits.y && bgFill[(int)Pos.x,(int)Pos.y] == null)
                    {
                        PopupCube cube = Instantiate(bgCube, Pos, Quaternion.identity, transform);
                        bgFill[(int)Pos.x, (int)Pos.y] = cube;
                        hasNew = true;
                    }
                }
            }
            yield return new WaitForSeconds(0.15f);
            counter++;
        }
        Debug.Log("OK");
        Controller.OnReady.Invoke();    
    }
}