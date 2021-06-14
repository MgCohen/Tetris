using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{

    List<Piece> nextPieces = new List<Piece>();

    [SerializeField]
    private DrawBag bag;

    private void Start()
    {
        InitialPreview();
    }

    public Piece NextPiece()
    {
        var piece = nextPieces[0];
        nextPieces.Remove(piece);
        DrawPiece();
        return piece;
    }

    void OrderPieces()
    {
        for (int i = 0; i < nextPieces.Count; i++)
        {
            nextPieces[i].transform.localPosition = new Vector3(1, 12 - (3 * i), 0);
        }
    }

    void DrawPiece()
    {
        var newPiece = Instantiate(bag.GetPiece(), transform);
        nextPieces.Add(newPiece);
        OrderPieces();
    }

    void InitialPreview()
    {
        DrawPiece();
        DrawPiece();
        DrawPiece();
        DrawPiece();
    }
}
