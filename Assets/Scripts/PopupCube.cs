using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupCube : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(0.95f, 0.25f).SetEase(Ease.OutBack);
    }

    public void Mark()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
