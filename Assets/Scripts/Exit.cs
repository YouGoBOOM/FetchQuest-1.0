using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    public string levelToLoad;          // Goes to level

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // EDIT THIS FUNCTION OR REMOVE COMPLETELY IN FUTURE
    // DO SOMETHING IN UPDATE SO THAT PLAYER MOVES INTO MIDDLE OF EXIT SPACE BEFORE LOADING
    // When player enters exit space, load specific scene
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            SceneManager.LoadScene(levelToLoad);
        }
    }

}
