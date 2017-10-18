using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject followTarget;             // Target to follow
    private Vector3 targetPosition;             // Position of target
    public float moveSpeed = 4;                 // Speed of camera
    public static bool cameraExists = false;    // Determines if the camera already exists

    // Use this for initialization
    void Start () {
        // Check between levels if player exists
        if (!cameraExists) {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Follow target and lag behind player slightly
        targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
