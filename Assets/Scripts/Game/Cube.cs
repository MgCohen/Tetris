using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class Cube : MonoBehaviour
{
    [SerializeField]
    [OnValueChanged("SwapMaterial")]
    Material material;

    public MeshRenderer[] meshes;

    public void SwapMaterial()
    {
#if UNITY_EDITOR
        foreach (MeshRenderer mesh in meshes) mesh.material = material;
#endif
    }
}
