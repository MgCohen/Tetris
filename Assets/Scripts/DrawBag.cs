using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Config/Bag")]
public class DrawBag : ScriptableObject
{
    [SerializeField]
    List<Piece> pieces = new List<Piece>();

    List<Piece> availablePieces = new List<Piece>();

    public Piece GetPiece()
    {
        if (availablePieces.Count <= 0) ShuffleBag();
        Piece nextPiece = availablePieces[0];
        availablePieces.Remove(nextPiece);
        return nextPiece;
    }

    void ShuffleBag()
    {
        foreach(Piece piece in pieces)
        {
            availablePieces.Add(piece);
        }

        availablePieces.Shuffle();
    }
}
