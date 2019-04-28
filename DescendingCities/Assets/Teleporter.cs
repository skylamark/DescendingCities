using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Teleporter : MonoBehaviour
{
    public Transform teleportTargetLocation;
    private GameObject objToTeleport;
    public GameObject uIFaderObj;
    private UI_Fader_Controller uI_Fader;
    public GameObject buttonTeleport;
    private Button button;


    // Start is called before the first frame update
    void Start()
    {
        button = buttonTeleport.GetComponent<Button>();
        uI_Fader = uIFaderObj.GetComponent<UI_Fader_Controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        objToTeleport = other.gameObject;
        buttonTeleport.SetActive(true);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ActivateTeleport());
    }

    private void OnTriggerExit(Collider other)
    {
        objToTeleport = null;
        buttonTeleport.SetActive(false);
    }

    public void ActivateTeleport()
    {
        uI_Fader.FadeIn();
        Invoke("TeleportObject", 1.5f);
        objToTeleport.GetComponent<NavMeshAgent>().enabled = false;
    }

    public void TeleportObject()
    {
        objToTeleport.transform.position = teleportTargetLocation.transform.position;
        objToTeleport.transform.rotation = teleportTargetLocation.transform.rotation;
        objToTeleport.GetComponent<NavMeshAgent>().enabled = true;
        objToTeleport = null;
        uI_Fader.FadeOut(0.5f);

    }

}
