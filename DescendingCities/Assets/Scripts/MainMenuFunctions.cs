using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class MainMenuFunctions : MonoBehaviour
{
    public GameObject Ui_LandingPage;
    public GameObject Ui_MakeReport;
    public GameObject Ui_AddUserData;
    public GameObject Ui_AddUserFile;
    public GameObject Ui_ConfirmSubmission;
    public GameObject Ui_ConfirmSend;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*///////// UI Page Navigation /////////*/

    public void UI_CloseAll()
    {
        Ui_LandingPage.SetActive(false);
        Ui_MakeReport.SetActive(false);
        Ui_AddUserData.SetActive(false);
        Ui_AddUserFile.SetActive(false);
        Ui_ConfirmSubmission.SetActive(false);
        Ui_ConfirmSend.SetActive(false);
        }

    public void UI_OpenLandingPage()
    { Ui_LandingPage.SetActive(true); }

    public void UI_OpenMakeReport()
    { Ui_MakeReport.SetActive(true); }

    public void UI_OpenAddUserData ()
    { Ui_AddUserData.SetActive(true); }

    public void UI_OpenAddUserFile()
    { Ui_AddUserFile.SetActive(true); }

    public void UI_OpenConfrimSubmission()
    { Ui_ConfirmSubmission.SetActive(true); }

    public void UI_OpenConfirmSend()
    { Ui_ConfirmSend.SetActive(true); }

    /*///////// Adventure Creator Dependant Functions /////////*/



    /*///////// Generic Functions /////////*/

    public void OpenURL(string string_url)
    {
        Application.OpenURL(string_url);
    }

}
