using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterMovement : MonoBehaviour
{

    Camera playerCamera;
    CharacterController playerController;

 
    public float moveDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
            playerController.Move(playerCamera.transform.rotation * new Vector3(0f, 0f, moveDistance * Time.deltaTime));

        if (Input.GetKey("s"))
            playerController.Move(playerCamera.transform.rotation * new Vector3(0f, 0f, -moveDistance * Time.deltaTime));

        if (Input.GetKey("a"))
            playerController.Move(playerCamera.transform.rotation * new Vector3(-moveDistance * Time.deltaTime, 0f, 0f));

        if (Input.GetKey("d"))
            playerController.Move(playerCamera.transform.rotation * new Vector3(moveDistance * Time.deltaTime, 0f, 0f));

        if (Input.GetKey("space"))
            playerController.Move(new Vector3(0f, moveDistance * Time.deltaTime, 0f));
    }



}
