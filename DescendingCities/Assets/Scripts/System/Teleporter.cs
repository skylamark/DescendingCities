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
    private float startTime;


    // Start is called before the first frame update
    void Start()
    {
        button = buttonTeleport.GetComponent<Button>();
        uI_Fader = uIFaderObj.GetComponent<UI_Fader_Controller>();
        startTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            objToTeleport = other.gameObject;

            startTime = Time.time;


            /* buttonTeleport.SetActive(true);
             button.onClick.RemoveAllListeners();
             button.onClick.AddListener(() => ActivateTeleport());*/
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Time.time - startTime >= 0.5)
            {
                
                ActivateTeleport();
                startTime = 0;
             
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //objToTeleport = null;
           // buttonTeleport.SetActive(false);
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
        Debug.Log("Teleported");
        objToTeleport.transform.position = teleportTargetLocation.transform.position;
        objToTeleport.transform.rotation = teleportTargetLocation.transform.rotation;
        //objToTeleport.GetComponent<NavMeshAgent>().enabled = true;
        uI_Fader.FadeOut(0.5f);

    }

}
