using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class UI_ControllerChoice : MonoBehaviour
{
    public Dropdown dropdown;
    public GameObject Joystick;
    public GameObject player;
    private PlayerController playerCntrl;
    private PlayerMovement playerMvmnt;
    private PlayerDirectional playerDrctnl;
    private KeyboardMovement keyboadMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerCntrl = player.GetComponent<PlayerController>();
        playerMvmnt = player.GetComponent<PlayerMovement>();
        playerDrctnl = player.GetComponent<PlayerDirectional>();
        keyboadMovement = player.GetComponent<KeyboardMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetController()
    {
        switch (dropdown.value)
        {
            case 0:
                Joystick.SetActive(true);
                player.GetComponent<NavMeshAgent>().ResetPath();
                playerCntrl.touchInput = false;     /* Android Only */
                playerDrctnl.enabled = true;
                playerCntrl.enabled = false;
                playerMvmnt.enabled = false;
                keyboadMovement.enabled = false;
                break;
            case 1:
                Joystick.SetActive(false);
                playerCntrl.touchInput = true;      /* Android Only */ 
                playerDrctnl.enabled = false;
                playerCntrl.enabled = true;
                playerMvmnt.enabled = true;
                keyboadMovement.enabled = false;
                break;
            case 2:
                Joystick.SetActive(false);
                player.GetComponent<NavMeshAgent>().ResetPath();
                playerCntrl.touchInput = false;      /* Desktop Only */
                playerDrctnl.enabled = false;
                playerCntrl.enabled = false;
                playerMvmnt.enabled = false;
                keyboadMovement.enabled = true;
                break;
        }


    }
}
