using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public Camera fpsCam;
    public Underwater underwater;

    public float speed = 12f;
    public float gravity = -9.82f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    float waterHeight;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        fpsCam = GetComponentInChildren<Camera>();
        underwater = GetComponent<Underwater>();
        waterHeight = underwater.GetWaterHeight();
    }

    

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (DialogueManager.instance.isInDialogue)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isUnderwater = underwater.IsUnderwater();

        Vector3 move = transform.right * x + transform.forward * z;
        if (isUnderwater)
        {
            move = fpsCam.transform.right * x + fpsCam.transform.forward * z;
            if (transform.position.y + move.y > waterHeight)
            {
                move.y = waterHeight - transform.position.y;
            }

            velocity.y += 0.5f;
            velocity.y = Mathf.Min(velocity.y, 0f);
        }

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && !isUnderwater)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if ((Input.GetButtonDown("Jump") || Input.GetButton("Jump")) && (Mathf.Abs((waterHeight - transform.position.y)) < 0.5f))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            controller.Move(velocity * Time.deltaTime);
        }


        if (Input.GetButton("Jump") && isUnderwater)
        {
            controller.Move(transform.up * speed * Time.deltaTime);
        }


        if (!isUnderwater)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

    }


}
