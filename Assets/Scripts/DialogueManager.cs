using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dialogueBox;                  // Getting the dialogue box
    public Text dialogueText;                       // Getting the dialogue text
    public bool dialogueActive;                     // Check if the dialogue box is active
    public DialogueHolder currentDialogueHolder;    // Getting the current dialogue holder
    public bool holdingDown;                        // Check if holding down any button
    public bool firstHeldDown;                      // Check if first held down right click

	// Use this for initialization
	void Start () {
        firstHeldDown = true;
	}
	
	// Update is called once per frame
	void Update () {
        // This replaces Input.anyKeyUp
        if (Input.anyKey) {
            holdingDown = true;
        }
        // Q to continue through text?
        // Basic remove text box function
        if (!Input.anyKey && holdingDown) {
            // This replaces Input.anyKeyUp
            holdingDown = false;
            // Check if dialogue is active
            if (dialogueActive) {
                // Check if first time lifting buttong
                if (firstHeldDown) {
                    // Remove dialogue box
                    dialogueBox.SetActive(false);
                    dialogueActive = false;
                    if (currentDialogueHolder != null) {
                        currentDialogueHolder.ResumeMovement();
                        currentDialogueHolder.dialogueOpened = false;
                    }
                } else {
                    firstHeldDown = true;
                }
            }
        }
	}

    // Function to show the dialogue box
    public void showBox(string dialogue) {
        dialogueActive = true;
        dialogueBox.SetActive(true);
        dialogueText.text = dialogue;
        firstHeldDown = false;
    }
}
