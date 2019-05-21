using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
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
    public Dropdown followDropdown;
    public Dropdown angleDropdown;

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
                    RealeaseMainCam();
                    cameras[0].tag = "MainCamera";
                    cameras[0].gameObject.SetActive(true);
                    cameras[1].gameObject.SetActive(false);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(false);
                    cameras[4].gameObject.SetActive(false);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
                case CameraAngle.HalfWay:
                    RealeaseMainCam();
                    cameras[1].tag = "MainCamera";
                    cameras[0].gameObject.SetActive(false);
                    cameras[1].gameObject.SetActive(true);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(false);
                    cameras[4].gameObject.SetActive(false);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
                case CameraAngle.TopDown:
                    RealeaseMainCam();
                    cameras[2].tag = "MainCamera";
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
                    RealeaseMainCam();
                    cameras[3].tag = "MainCamera";
                    cameras[0].gameObject.SetActive(false);
                    cameras[1].gameObject.SetActive(false);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(true);
                    cameras[4].gameObject.SetActive(false);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
                case CameraAngle.HalfWay:
                    RealeaseMainCam();
                    cameras[4].tag = "MainCamera";
                    cameras[0].gameObject.SetActive(false);
                    cameras[1].gameObject.SetActive(false);
                    cameras[2].gameObject.SetActive(false);
                    cameras[3].gameObject.SetActive(false);
                    cameras[4].gameObject.SetActive(true);
                    cameras[5].gameObject.SetActive(false);
                    privateAngle = cameraAngle;
                    break;
                case CameraAngle.TopDown:
                    RealeaseMainCam();
                    cameras[5].tag = "MainCamera";
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

    // DropDown Window functions for playtest purposes

    public void SetFollowStateWithButton()
    {
        switch (followDropdown.value)
        {
            case 0:
                followPlayerSetting = FollowPlayer.RotateWithPlayer;
                break;
            case 1:
                followPlayerSetting = FollowPlayer.FixedWorldAngle;
                break;
            default:
                followPlayerSetting = FollowPlayer.RotateWithPlayer;
                break;
        }

    }

    public void SetAngleStateWithButton()
    {
        switch (angleDropdown.value)
        {
            case 0:
                cameraAngle = CameraAngle.Sideview;
                break;
            case 1:
                cameraAngle = CameraAngle.HalfWay;
                break;
            case 2:
                cameraAngle = CameraAngle.TopDown;
                break;
            default:
                cameraAngle = CameraAngle.HalfWay;
                break;
        }

    }

    private void RealeaseMainCam()
    {
        cameras[0].tag = "Untagged";
        cameras[1].tag = "Untagged";
        cameras[2].tag = "Untagged";
        cameras[3].tag = "Untagged";
        cameras[4].tag = "Untagged";
        cameras[5].tag = "Untagged";

    }

}
