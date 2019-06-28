using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;
using TMPro;

public class TutorialEnforcer : MonoBehaviour
{
    public enum TutorialState { Off, Start, Active, NextStep, Done}
    public TutorialState tutorialState;
    private TutorialState privState;
    public GameManager managerObj;
    private bool tutorialActive;
    private int tutorialStep = 0;
    public bool stepComplete = false;

    [TextArea]
    public string[] infoMessages;
    // Start is called before the first frame update
    void Start()
    {
        if (managerObj.forceTutorial)
        {
            tutorialActive = true;
        }
        else {
            tutorialActive = false;
            tutorialState = TutorialState.Off;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (tutorialState != privState)
        {
            switch (privState)
            {
                case TutorialState.Off:
                    break;
                case TutorialState.Start:
                    ExecuteStep();
                    break;
                case TutorialState.Active:
                    if (stepComplete)
                    {
                    }
                    break;
                case TutorialState.NextStep:
                    NextStep();
                    break;
                case TutorialState.Done:

                    break;
                default:
                    return;
                    break;
            }
        }

    }

    void ExecuteStep()
    {
        TextMeshProUGUI _tempString = managerObj.informationContent.GetComponent<TextMeshProUGUI>();
        _tempString.text = infoMessages[tutorialStep];
        managerObj.information.SetActive(true);
        tutorialState = TutorialState.Active;
    }

    void NextStep()
    {
        tutorialState = TutorialState.Start;
    }

   

}
