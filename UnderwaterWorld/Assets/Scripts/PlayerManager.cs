using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerManager found!");
        }
        instance = this;
    }

    #endregion


    public GameObject player;

    PlayerMovement playerMove;
    PlayerWaterMovement playerWaterMove;

    private void Start()
    {
        playerMove = player.GetComponent<PlayerMovement>();
        playerWaterMove = player.GetComponent<PlayerWaterMovement>();
    }


    public void SwapToPlayerWaterMovementType()
    {

        if (playerMove.enabled)
        {
            playerMove.enabled = false;
            playerWaterMove.enabled = true;
        }

    }

    public void SwapToPlayerMovementType()
    {

        if (!playerMove.enabled)
        {
            playerMove.enabled = true;
            playerWaterMove.enabled = false;
        }

    }



}
