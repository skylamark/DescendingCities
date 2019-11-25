using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public Transform teleportTargetLocation;
    public GameObject objToTeleport;
    public GameObject uIFaderObj;
    private UI_Fader_Controller uI_Fader;

    private void Start()
    {
        uI_Fader = uIFaderObj.GetComponent<UI_Fader_Controller>();
    }
    public void EnterHome()
    {
        if (objToTeleport != null) { ActivateTeleport(); }
        else {
            objToTeleport = GameObject.Find("AVC_Player");
            ActivateTeleport();
        }
    }

    public void ActivateTeleport()
    {
        uI_Fader.FadeIn();
        Invoke("TeleportObject", 1.5f);
        //objToTeleport.GetComponent<NavMeshAgent>().enabled = false;
    }

    public void TeleportObject()
    {
        objToTeleport.transform.position = teleportTargetLocation.transform.position;
        objToTeleport.transform.rotation = teleportTargetLocation.transform.rotation;
        //objToTeleport.GetComponent<NavMeshAgent>().enabled = true;
        uI_Fader.FadeOut(0.5f);

    }

}
