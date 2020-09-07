using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 5f;

    public Camera fpsCam;
    public GameObject interactBox;
    private Text interactText;

    // Start is called before the first frame update
    void Start()
    {
        interactText = interactBox.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused || DialogueManager.instance.isInDialogue)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, interactRange))
        {
            //Debug.Log("Looking at " + hit.transform.name);
            Interactable target = hit.transform.GetComponent<Interactable>();
            if (target != null)
            {
                interactBox.SetActive(true);
                interactText.text = target.lookAtText;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactBox.SetActive(false);
                    target.Interact();
                }

            }

        } else
        {
            interactBox.SetActive(false);
        }

    }
}
