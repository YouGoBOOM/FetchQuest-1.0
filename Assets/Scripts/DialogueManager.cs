using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dialogueBox;                  // Getting the dialogue box
    public Text dialogueText;                       // Getting the dialogue text
    public bool dialogueActive;                     // Check if the dialogue box is active
    public DialogueHolder currentDialogueHolder;    // Getting the current dialogue holder
    public MouseController theCursor;               // Getting the mouse controller
    //public PlayerController thePlayer;              // Getting the player
    public string[] dialogueLines;                  // Getting the dialogue lines
    public int currentDialogueLine;                 // Getting the current dialogue line

    // Use this for initialization
    void Start () {
        theCursor = FindObjectOfType<MouseController>();    // Getting the mouse
        //thePlayer = FindObjectOfType<PlayerController>();   // Getting the player
    }
	
	// Update is called once per frame
	void Update () {
        if (dialogueActive) {
            if (Input.GetKeyUp(KeyCode.Space)) {
                // Go through the dialogue array
                currentDialogueLine++;
                if (currentDialogueLine >= dialogueLines.Length) {
                    // Remove dialogue box
                    SetDialogueInactive();
                    currentDialogueLine = 0;
                }
                // Show dialogue
                dialogueText.text = dialogueLines[currentDialogueLine];
            }
        }
	}

    // Function to show the dialogue box
    public void ShowDialogue(string[] lines) {
        currentDialogueLine = 0;
        dialogueLines = lines;
        dialogueActive = true;
        dialogueBox.SetActive(true);
        dialogueText.text = dialogueLines[currentDialogueLine];
    }

    // Function to set dialogue inactive
    public void SetDialogueInactive() {
        dialogueBox.SetActive(false);
        dialogueActive = false;
        if (currentDialogueHolder != null) {
            currentDialogueHolder.ResumeMovement();
            currentDialogueHolder.dialogueOpened = false;
        }
        theCursor.targetingNPC = false;
    }
}
