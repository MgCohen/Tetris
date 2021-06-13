using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Cube[] cubes = new Cube[4];

    public void Move(Vector3Int direction)
    {
        transform.position += direction;
    }

    public void Rotate(int direction)
    {
        float rotation = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation + (90 * direction)));
    }
}
