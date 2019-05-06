using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueSystem : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;

    public GameObject dialogueGUI;
    public GameObject acceptDeclineButtons;
    public Transform dialogueBoxGUI;

    public float letterDelay = 0.1f;
    public float letterMultiplier = 0.5f;

    

    public string Names;

   
    public string[] dialogueLines;

    public bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true;
    public Owner currentOwner;

    public AudioClip audioClip;
    AudioSource audioSource;

  

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dialogueText.text = "";

    }

    void Update()
    {
        if (outOfRange)
        {
            dialogueGUI.SetActive(false);
            
        }
    }

    public void EnterRangeOfNPC()
    {
        outOfRange = false;
        dialogueGUI.SetActive(true);
        if (dialogueActive == true)
        {
            dialogueGUI.SetActive(false);
        }

       
    }

    public void NPCName()
    {
        outOfRange = false;
        dialogueBoxGUI.gameObject.SetActive(true);
        nameText.text = Names;
        if (Input.GetMouseButtonDown(0)) ;
        {
            if (!dialogueActive)
            {
                dialogueActive = true;
                StartCoroutine(StartDialogue());
            }
        }
        StartDialogue();
    }

    private IEnumerator StartDialogue()
    {
       
        if (outOfRange == false)
        {
            int currentDialogueIndex = 0;
            int dialogueLength = dialogueLines.Length;
            

            while (currentDialogueIndex < dialogueLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++]));

                    if(currentDialogueIndex == dialogueLength - 1)
                    {

                        if (currentOwner.Task.Status == 0 && PlayerManagement.player.isAvailable())
                        {
                            acceptDeclineButtons.SetActive(true);

                        }
                    }

                    if (currentDialogueIndex >= dialogueLength)
                    {

                        dialogueEnded = true;
                    }
                }
                yield return 0;
            }

            while (true)
            {
                if (Input.GetMouseButtonDown(0) && dialogueEnded == false)
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            dialogueActive = false;
            DropDialogue();
        }
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        if (outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogueText.text = "";

            while (currentCharacterIndex < stringLength)
            {
                dialogueText.text += stringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;

                if (currentCharacterIndex < stringLength)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);

                        if (audioClip) audioSource.PlayOneShot(audioClip, 0.5F);
                    }
                    else
                    {
                        yield return new WaitForSeconds(letterDelay);

                        if (audioClip) audioSource.PlayOneShot(audioClip, 0.5F);
                    }
                }
                else
                {
                    dialogueEnded = false;
                    break;
                }
            }
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueText.text = "";
        }
    }

    public void DropDialogue()
    {
     
        dialogueGUI.SetActive(false);
        dialogueBoxGUI.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        
        outOfRange = true;
        if (outOfRange == true)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            dialogueGUI.SetActive(false);
            dialogueBoxGUI.gameObject.SetActive(false);
        }
    }

    public void Accept()
    {
        Debug.Log("Accepted");
        currentOwner.task.status = 1;
            PlayerManagement.player.CurrTask = currentOwner.task;
            acceptDeclineButtons.SetActive(false);

    }
    public void Decline()
    {
        currentOwner.task.status = 0;
        acceptDeclineButtons.SetActive(false);
        Debug.Log("Declined");

    }
}
