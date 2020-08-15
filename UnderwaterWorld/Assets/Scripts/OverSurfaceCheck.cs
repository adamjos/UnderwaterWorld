using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverSurfaceCheck : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.instance.SwapToPlayerMovementType();
    }


}
