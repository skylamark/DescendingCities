using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    Camera camera;
    public LayerMask walkableMask;
    PlayerMovement movement;

   

    void Start()
    {
        camera = Camera.main;
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayCastHit;

            if(Physics.Raycast(ray, out rayCastHit, 100, walkableMask))
            {
                movement.MoveToPoint(rayCastHit.point);
           
            }
        }
    }
}
