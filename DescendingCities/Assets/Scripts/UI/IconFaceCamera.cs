using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFaceCamera : MonoBehaviour
{
    private Transform camera;
    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        if (!camera)
        { camera = GameObject.FindWithTag("MainCamera").gameObject.transform; }
        if (!renderer)
        { renderer = gameObject.GetComponent<SpriteRenderer>(); }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera);
    }
}
