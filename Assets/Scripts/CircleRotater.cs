using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotater : MonoBehaviour
{
    public float _rotateSpeed = 100f;
    void Update()
    {
        transform.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
    }
}