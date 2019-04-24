using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogeUIText : MonoBehaviour {

    [Header("Dialoge Box GameObject")]
    public GameObject thisDialogueBox;
    public Image thisDialogueBoxImage;
    public TMP_Text thisDialogueBoxText;
    private Animator boxAnimator;
    public GameObject nextDialogue;
    public GameObject closeDialogue;
    private int activeConv = 0;
    private int activeConvProgress = 0;
    private bool areTalking = false;
    //private MainSceneHandler _msh;
    public GameObject msh;
    private bool tutorial = false;

    [Header("Dialogue Anna")]
    public Sprite imageAnna;
    [TextArea]
    public String[] annaDialogueText;

    [Header("Dialogue Nurse")]
    public Sprite imageNurse;
    [TextArea]
    public String[] nurseDialogueText;

    [Header("Dialogue Narrator")]
    public Sprite imageNarrator;
    [TextArea]
    public String[] narratorDialogueText;

    [Header("Dialogue Sequences")]
    [Header("(0-999 Anna | 1000-1999 Nurse | 2000-2999 Narrator")]
    [Header("!Leave Element 0 Empty!")]
    public ConversationsList[] conversationID;
    


    // Use this for initialization
    void Start () {
        boxAnimator = thisDialogueBox.GetComponent<Animator>();
        /*if(msh == null) {
            msh = GameObject.FindGameObjectWithTag("MSH");
            _msh = msh.GetComponent<MainSceneHandler>();
        }
        if (GameData.currentCharacter.tutorial)
        { tutorial = true; }*/
	}
	
	// Update is called once per frame
	void Update () {
        if (activeConv != 0 && areTalking)
        {
            OpenDialogeBox();
            areTalking = false;
        }
        /*if (msh == null)
        {
            msh = GameObject.FindGameObjectWithTag("MSH");
            _msh = msh.GetComponent<MainSceneHandler>();
        }*/
    }

    void OpenDialogeBox()       // Activate dialogue box animation to show it on screen
    {
        boxAnimator.SetBool("DialogueBox_InOut", true);
        /*if (!tutorial)
        {
            _msh.CloseAll();
        }*/
    }

    public void CloseDialogueBox()                     // Deactivate dialogue box animation to remove it from screen
    {
        boxAnimator.SetBool("DialogueBox_InOut", false);
        Invoke("CleanDialogueBoxText", 0.3f);
        /* if (!tutorial)
        {
            _msh.OpenAll();
        }
        else
        { GameState.currentState = State.START; }
        */
    }

    void CleanDialogueBoxText()
    {
        thisDialogueBoxText.text = "";
        nextDialogue.SetActive(false);
        closeDialogue.SetActive(false);
        activeConv = 0;
        activeConvProgress = 0;
        //Debug.Log("Dialogue Box Cleaned, Conversation Int's set to 0");
    }

    void SetDialogueText(int _i)
    {
        if (conversationID[_i].dialogueSequence[activeConvProgress] < 1000)         // If  below 1000 then text is said by Chloe.
        {
            thisDialogueBoxText.text = "Anna: " + annaDialogueText[conversationID[_i].dialogueSequence[activeConvProgress]]; // Get int from Dialogue Sequence, 
            thisDialogueBoxImage.sprite = imageAnna;                               // Set image to Chloe's.
        }
        else if  (conversationID[_i].dialogueSequence[activeConvProgress] < 2000)                                                                     // else text is said by Nurse.
        {
            int _temp = conversationID[_i].dialogueSequence[activeConvProgress] - 1000;
            thisDialogueBoxText.text = "Verpleegkundige: " + nurseDialogueText[_temp];
            thisDialogueBoxImage.sprite = imageNurse;
        }
        else
        {
            int _temp = conversationID[_i].dialogueSequence[activeConvProgress] - 2000;
            thisDialogueBoxText.text = "Verteller " + narratorDialogueText[_temp];
            thisDialogueBoxImage.sprite = imageNarrator;
        }
    }

    public void SetConversation(int _conv)
    {
        activeConvProgress = 0;
        if (_conv != 0)
        {
            SetDialogueText(_conv);                                                 // Sets Text and Image for Dialogue Box.
            if (activeConvProgress < (conversationID[_conv].dialogueSequence.Length-1)) // If Active Conv Progress is smaller then length of Dialogue Sequence: turn on "Next"-Button, Else: turn on "End Convo"-Button.
            {
                nextDialogue.SetActive(true);                                       // If coversation progress is lower then the length of the sequence, then show "Next"-Button.
            }
            else if (activeConvProgress == (conversationID[_conv].dialogueSequence.Length-1))
            {
                closeDialogue.SetActive(true);                                      // If coversation progress is lower then the length of the sequence, then show "Next"-Button.
            }
            areTalking = true;
        }
        activeConv = _conv;
    }

    public void NextDialogue()
    {
        int _temp = (conversationID[activeConv].dialogueSequence.Length - 2);
        if (activeConvProgress < _temp) // If Active Conv Progress is smaller then length of Dialogue Sequence: turn on "Next"-Button, Else: turn on "End Convo"-Button.
        {
            nextDialogue.SetActive(true);                                       // If coversation progress is lower then the length of the sequence, then show "Next"-Button.
            closeDialogue.SetActive(false);
        }
        else if (activeConvProgress == _temp)
        {
            nextDialogue.SetActive(false);                                      // If coversation progress is lower then the length of the sequence, then show "Next"-Button.
            closeDialogue.SetActive(true);
        }
        activeConvProgress += 1;
        SetDialogueText(activeConv);
    }    
}