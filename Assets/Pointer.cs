using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Pointer : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro pointText;

    public int points => _points;
    private int _points;


    /// <summary>
    /// Score lines, number of lines square * 10
    /// </summary>
    public void Score(int amount)
    {
        _points += (amount * amount * 10);
        pointText.text = _points.ToString();
    }
}
