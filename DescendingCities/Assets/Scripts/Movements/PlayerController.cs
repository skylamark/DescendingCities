using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    Camera camera;
    public LayerMask walkableMask;
    PlayerMovement movement;
    public bool touchInput;

   

    void Start()
    {
        camera = Camera.main;
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!touchInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayCastHit;

                if (Physics.Raycast(ray, out rayCastHit, 100, walkableMask))
                {
                    movement.MoveToPoint(rayCastHit.point);

                }
            }
        }
        else {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                    RaycastHit rayCastHit;

                    if (Physics.Raycast(ray, out rayCastHit, 100, walkableMask))
                    {
                        movement.MoveToPoint(rayCastHit.point);
                        Debug.Log("Raycast fired by touch");
                    }
                }
            }
        }
    }
}
