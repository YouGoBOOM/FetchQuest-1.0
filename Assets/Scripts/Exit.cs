using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    public string levelToLoad;              // Goes to level
    private GameObject exitHover;           // Getting the exit hover
    public string exitPoint;                // Getting specific exit point
    private PlayerController thePlayer;     // Getting the player

    // Use this for initialization
    void Start () {
        exitHover = transform.GetChild(0).gameObject;               // Getting the exit hover
        exitHover.SetActive(false);                                 // Disable the exit hover
        // Getting the player
        thePlayer = GameObject.FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            // When player enters exit space, set current exit to this exit
            thePlayer.currentExit = gameObject;
            thePlayer.startPoint = exitPoint;
        }
        if (other.gameObject.name == "Cursor") {
            // When mouse hovers over exit, activate exitHover
            exitHover.SetActive(true);
            // targeting object is the exit
            other.gameObject.GetComponent<MouseController>().targetingObject = transform.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            // When player exits exit space, set current exit to null
            other.gameObject.GetComponent<PlayerController>().currentExit = null;
        }
        if (other.gameObject.name == "Cursor") {
            // Disable exitHover when cursor goes off
            exitHover.SetActive(false);
            // targeting object is no longer the exit
            other.gameObject.GetComponent<MouseController>().targetingObject = null;
        }
    }

    public void MoveToLevel() {
        SceneManager.LoadScene(levelToLoad);
    }
}
