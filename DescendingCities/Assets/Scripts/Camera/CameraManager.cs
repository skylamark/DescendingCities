using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public enum FollowPlayer { RotateWithPlayer, FixedWorldAngle };
    public enum CameraAngle { Sideview,HalfWay,TopDown };
    public FollowPlayer followPlayerSetting;
    public CameraAngle cameraAngle;
    public GameObject playerObj;
    public Camera[] cameras;
    private bool update;
    private FollowPlayer privateFollow;
    private CameraAngle privateAngle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (privateAngle != cameraAngle)
        { update = true; }
        if (privateFollow != followPlayerSetting)
        { update = true; }

        if (update)
        {
            SetCamera();
        }

    }

    void SetCamera()
    {
        if (followPlayerSetting ==FollowPlayer.RotateWithPlayer)
        {
            switch (cameraAngle)
            {
                case CameraAngle.Sideview:
                    cameras[0].gameObject.SetActive(true);
                    cameras[1].gameObject.SetActive(false);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(false);
                    cameras[4].gameObject.SetActive(false);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
                case CameraAngle.HalfWay:
                    cameras[0].gameObject.SetActive(false);
                    cameras[1].gameObject.SetActive(true);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(false);
                    cameras[4].gameObject.SetActive(false);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
                case CameraAngle.TopDown:
                    cameras[0].gameObject.SetActive(false);
                    cameras[1].gameObject.SetActive(false);
                    cameras[2].gameObject.SetActive(true);
                    cameras[3].gameObject.SetActive(false);
                    cameras[4].gameObject.SetActive(false);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
            }
            privateFollow = followPlayerSetting;
        }
        else if (followPlayerSetting == FollowPlayer.FixedWorldAngle)
        {
            switch (cameraAngle)
            {
                case CameraAngle.Sideview:
                    cameras[0].gameObject.SetActive(false);
                    cameras[1].gameObject.SetActive(false);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(true);
                    cameras[4].gameObject.SetActive(false);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
                case CameraAngle.HalfWay:
                    cameras[0].gameObject.SetActive(false);
                    cameras[1].gameObject.SetActive(false);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(false);
                    cameras[4].gameObject.SetActive(true);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
                case CameraAngle.TopDown:
                    cameras[0].gameObject.SetActive(false);
                    cameras[1].gameObject.SetActive(false);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(false);
                    cameras[4].gameObject.SetActive(false);
                    cameras[5].gameObject.SetActive(true);
                    privateAngle = cameraAngle;
                    break;
            }
            privateFollow = followPlayerSetting;
        }
        update = false;
    }

    public void DisableAllCams()
    {
        int _i;
        for(_i=0;_i<cameras.Length;_i++)
        {
            cameras[_i].gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        transform.position = playerObj.transform.position;
    }
}
