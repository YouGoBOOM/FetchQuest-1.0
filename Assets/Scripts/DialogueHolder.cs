using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour {

    public string[] dialogueLines;                  // Dialogue lines
    public DialogueManager dialogueManager;         // Getting the dialogue manager
    public float readableDistance;                  // Getting the distance required to dialogue 
    public bool dialogueOpened;                     // Check if the dialogue is opened

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Open dialogue
    public void OpenDialogue() {
        // Check if dialogue held by NPC
        if (gameObject.tag == "NPC") {
            GetComponent<NPCMovement>().pauseWalkingForDialogue = true;
        }
        dialogueManager.currentDialogueHolder = this;
        // Open dialogue if not already
        if (!dialogueOpened) {
            dialogueManager.ShowDialogue(dialogueLines);
            dialogueOpened = true;
        }
    }

    // Resume Movement
    public void ResumeMovement() { 
        // Check if dialogue held by NPC
        if (gameObject.tag == "NPC") {
            GetComponent<NPCMovement>().pauseWalkingForDialogue = false;
        }
    }
}
