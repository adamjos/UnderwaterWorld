using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject player;

    PlayerMovement playerMove;
    PlayerWaterMovement playerWaterMove;

    public LayerMask waterMask;

    private void Start()
    {
        playerMove = player.GetComponent<PlayerMovement>();
        playerWaterMove = player.GetComponent<PlayerWaterMovement>();
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("We hit something");

        if (hit.gameObject.layer == waterMask)
        {
            SwapPlayerMovementType();
        }
    }



    public void SwapPlayerMovementType()
    {

        if (playerMove.enabled)
        {
            playerMove.enabled = false;
            playerWaterMove.enabled = true;
        }

    }


}
