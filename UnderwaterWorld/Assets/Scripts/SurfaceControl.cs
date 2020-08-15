using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceControl : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //PlayerManager.instance.SwapToPlayerWaterMovementType();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //PlayerManager.instance.SwapToPlayerMovementType();
        }
        
    }
}
