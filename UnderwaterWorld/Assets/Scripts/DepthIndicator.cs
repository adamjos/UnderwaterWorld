using UnityEngine;
using UnityEngine.UI;

public class DepthIndicator : MonoBehaviour
{

    public Text depthText;
    GameObject player;
    Vector3 playerPosition;
    Underwater underwater;


    void Start()
    {
        player = PlayerManager.instance.player;
        underwater = player.GetComponent<Underwater>();
    }

    // Update is called once per frame
    void Update()
    {
        depthText.text = (Mathf.Floor(underwater.waterHeight - player.transform.position.y)).ToString();
    }
}
