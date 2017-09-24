using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    public string levelToLoad;              // Goes to level
    private MouseController theMouse;       // Getting the mouse controller
    private GameObject exitHover;           // Getting the exitHover

    // Use this for initialization
    void Start () {
        exitHover = transform.GetChild(0).gameObject;               // Getting the exit hover
        exitHover.SetActive(false);                                 // Disable the exit hover
        theMouse = FindObjectOfType<MouseController>();             // Getting the mouse

    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D other) {
        // EDIT THIS FUNCTION OR REMOVE COMPLETELY IN FUTURE
        // DO SOMETHING IN UPDATE SO THAT PLAYER MOVES INTO MIDDLE OF EXIT SPACE BEFORE LOADING
        // When player enters exit space, load specific scene
        if (other.gameObject.name == "Player") {
            SceneManager.LoadScene(levelToLoad);
        }
        // When mouse hovers over exit, activate exitHover
        if (other.gameObject.name == "Cursor") {
            exitHover.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        // Disable exitHover when cursor goes off
        if (other.gameObject.name == "Cursor") {
            exitHover.SetActive(false);
        }
    }
}
