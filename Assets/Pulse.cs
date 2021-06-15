using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pulse : MonoBehaviour
{

    private void Start()
    {
        Down();
    }

    public void Up()
    {
        transform.DOScale(1f, 1f).OnComplete(Down);
    }

    public void Down()
    {
        transform.DOScale(0.9f, 1f).OnComplete(Up);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
