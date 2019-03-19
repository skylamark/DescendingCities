using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float zoom;
    public float playerHeight = 2f;
    void Start()
    {
        
    }
    
    void Update()
    {
        transform.position = target.position - offset * zoom;
        transform.LookAt(target.position + Vector3.up * playerHeight);

    }
}
