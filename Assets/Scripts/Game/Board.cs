using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public Vector2 limits = new Vector2(10, 20);
    public Cube[,] boardFill;
    public PopupCube[,] bgFill;
    public PopupCube bgCube;

    public Config config;

    private void Start()
    {
        limits = config.boardSize;
        boardFill = new Cube[(int)limits.x, (int)limits.y + 4];
        bgFill = new PopupCube[(int)limits.x, (int)limits.y];
        StartCoroutine(BackgroundCO());
    }

    /// <summary>
    /// Verifies if the piece is currently on a valid space before placing
    /// </summary>
    public bool CheckMove(Piece piece)
    {
        foreach (Cube cube in piece.cubes)
        {
            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(cube.transform.position.x), Mathf.RoundToInt(cube.transform.position.y));

            //check if cube is inside valid area
            if (pos.x < 0 || pos.x >= limits.x || pos.y < 0)
            {
                return false;
            }

            //check if there is anything on the target space
            if (boardFill[(int)pos.x, (int)pos.y] != null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Place a piece and all its cubes on the grid, marking the position.
    /// </summary>
    public void PlacePiece(Piece piece)
    {
        foreach (Cube cube in piece.cubes)
        {
            Vector2 piecePos = cube.transform.position;
            Vector2Int movePos = new Vector2Int(Mathf.RoundToInt(piecePos.x), Mathf.RoundToInt(piecePos.y));
            boardFill[movePos.x, movePos.y] = cube;
            if (movePos.x >= 0 && movePos.x < limits.x && movePos.y >= 0 && movePos.y < limits.y)
            {
                bgFill[movePos.x, movePos.y].Mark();
            }
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

        DestroyLine(height);
        return true;
    }

    /// <summary>
    /// Destroy all cubes in a set height
    /// </summary>
    void DestroyLine(int height)
    {
        for (int i = 0; i < limits.x; i++)
        {
            Destroy(boardFill[i, height].gameObject);
            boardFill[i, height] = null;
            bgFill[i, height].Unmark();
        }
    }

    /// <summary>
    /// Makes all cubes above a set height to fall
    /// </summary>
    public void ActivateGravity(int startingHeight, int gravityAmount)
    {
        for (int i = startingHeight + gravityAmount; i < limits.y; i++)
        {
            for (int j = 0; j < limits.x; j++)
            {
                var cube = boardFill[j, i];
                if (cube == null) continue;
                bgFill[j, i].Unmark();
                cube.transform.position += Vector3Int.down * gravityAmount;
                boardFill[j, i] = null;
                boardFill[j, i - gravityAmount] = cube;
                bgFill[j, i - gravityAmount].Mark();
            }
        }
    }

    /// <summary>
    /// Generic, poor implementation of flood-fill just for show
    /// </summary>
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
                    if (Pos.x >= 0 && Pos.x < limits.x && Pos.y >= 0 && Pos.y < limits.y && bgFill[(int)Pos.x, (int)Pos.y] == null)
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
