using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    public float mass = 3f;

    private Vector3 impact = Vector3.zero;
    private CharacterController playerController;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (impact.magnitude > 0.2f)
        {
            playerController.Move(impact * Time.deltaTime);
        }

        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    public void AddImpact (Vector3 force)
    {
        impact += force / mass;
    }

}
