using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraPerspectiveSettings : MonoBehaviour
{
    public GameObject cameraTarget;
    public enum CameraOptions { TopDown, HalfAngle, SideView, Custom};
    public CameraOptions cameraOptions;
    private CameraOptions privateState;

    [Header("Current Camera Settings")]
    public Vector2 positionOffset;
    public float xRotation;
    public float distance;

    private Vector2 cameraPosOffset; // this value actually affects the X,Y transform.
    private float cameraXRotation;  // this value actually affects the X rotation.
    private float cameraDistance;   // this value actually affects Z transform.

    private Transform _target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTarget != null && _target == null) {
            _target = cameraTarget.GetComponent<Transform>();
        }

        if (cameraOptions != privateState)
        {
            ChangeCameraSettings();
        }

    }

    private void ApplyCameraChanges()
    {
        if (cameraOptions== CameraOptions.Custom)             // If custom camera is active, the public values must drive the private values.
        {
            _target.transform.rotation = Quaternion.Euler(xRotation, 0, 0);                              // apply rotation changes to camera.
            transform.localPosition = new Vector3(positionOffset.x, positionOffset.y, -distance);   //apply transform changes to camera.
            cameraPosOffset = positionOffset;                                                               // tranfer values to public values for inspector.
            cameraDistance = -distance;
            cameraXRotation = xRotation;
        }
        else
        {
            _target.transform.rotation = Quaternion.Euler(cameraXRotation, 0, 0);                             // apply rotation changes to camera.
            transform.localPosition = new Vector3(cameraPosOffset.x, cameraPosOffset.y, -cameraDistance);   //apply transform changes to camera.
            positionOffset = cameraPosOffset;                                                              // tranfer values to public values for inspector.
            distance = -cameraDistance;
            xRotation = cameraXRotation;
        }
    }

    private void ChangeCameraSettings()
    {
        switch (cameraOptions)
        {
            case CameraOptions.Custom:
                //positionOffset = new Vector2(0,0);
                //xRotation = -90f;
                //distance = 10f;
                ApplyCameraChanges();
                break;
            case CameraOptions.HalfAngle:
                cameraPosOffset = new Vector2(0,0);
                cameraXRotation = 45f;
                cameraDistance = 10f;
                ApplyCameraChanges();
                privateState = CameraOptions.HalfAngle;
                break;
            case CameraOptions.SideView:
                cameraPosOffset = new Vector2(0,0);
                cameraXRotation = 0f;
                cameraDistance = 10f;
                ApplyCameraChanges();
                privateState = CameraOptions.SideView;
                break;
            case CameraOptions.TopDown:
                cameraPosOffset = new Vector2(0,0);
                cameraXRotation = 90f;
                cameraDistance = 10f;
                ApplyCameraChanges();
                privateState = CameraOptions.TopDown;
                break;
        }
    }
}
