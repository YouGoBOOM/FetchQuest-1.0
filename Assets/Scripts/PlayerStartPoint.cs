using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour {

    private PlayerController thePlayer;        // Getting the player
    private CameraController theCamera;        // Getting the camera
    public float startDirection;               // Set direction on start

	// Use this for initialization
	void Start () {
        // Set start position and direction of player
        thePlayer = FindObjectOfType<PlayerController>();
        thePlayer.transform.position = transform.position;
        thePlayer.mouseWorldSpace = transform.position;
        thePlayer.lastDirection = startDirection;
        // Getting solid layer collider for player
        thePlayer.solidCollider = GameObject.FindGameObjectWithTag("Solid").GetComponent<PolygonCollider2D>();
        // Set start position of camera
        theCamera = FindObjectOfType<CameraController>();
        theCamera.transform.position = new Vector3(transform.position.x, transform.position.y, theCamera.transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
