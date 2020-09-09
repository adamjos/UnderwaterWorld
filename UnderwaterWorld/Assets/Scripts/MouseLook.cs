using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public float recoilSpeed = 20f;

    public Transform playerBody;

    float xRotation = 0f;

    private float upRecoil = 0f;
    private float sideRecoil = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.instance.isInDialogue)
        {
            return;
        }

        // Multiply with delta time so that look around speed becomes independent of frame rate 
        float mouseX = sideRecoil + Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = upRecoil + Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        sideRecoil -= recoilSpeed * Time.deltaTime;
        upRecoil -= recoilSpeed * Time.deltaTime;

        if (sideRecoil < 0f)
        {
            sideRecoil = 0f;
        }

        if (upRecoil < 0f)
        {
            upRecoil = 0f;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void AddRecoil (float up, float side)
    {
        upRecoil += up;
        sideRecoil += side;
    }
}
