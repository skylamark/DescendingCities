using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    public enum buildPlatform { androidBuild , pcBuild };
    [Header("Build Settings")]
    public buildPlatform platform;
    private buildPlatform privPlatform;

    [Header("Player stuff")]
    public GameObject playerObj;
    public bool forceTutorial;
    public GameObject cameraObj;
    private new Camera camera;
    public GameObject cameraMiniMap;

    [Header("Buttons")]
    public GameObject homeBttn;
    public GameObject interactionBttn;
    public GameObject progressBarBttn;
    public GameObject minimapBttn;

    [Header("UI Elements")]
    public GameObject joystick;
    public GameObject progressBar;
    public GameObject toDo;
    public GameObject minimap;
    public GameObject helpScreen;
    public GameObject fader;
    public GameObject pcHelp;
    public GameObject androidHelp;
    public GameObject information;





    // Start is called before the first frame update
    void Start()
    {
        camera = cameraObj.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (privPlatform != platform)
        {
            UpdateBuildSettings();
            privPlatform = platform;
        }
    }

    private void LateUpdate()
    {
        cameraObj.transform.position = playerObj.transform.position;
        cameraMiniMap.transform.position = playerObj.transform.position;
    }

    /* Build check */
    void UpdateBuildSettings()
    {
        if (platform == buildPlatform.androidBuild) {
            playerObj.GetComponent<KeyboardMovement>().enabled = false;
            playerObj.GetComponent<PlayerDirectional>().enabled = true;

            joystick.SetActive(true);

        }
        if (platform == buildPlatform.pcBuild)
        {
            
            playerObj.GetComponent<KeyboardMovement>().enabled = true;
            playerObj.GetComponent<PlayerDirectional>().enabled = false;
            joystick.SetActive(false);
        }

    }
}
