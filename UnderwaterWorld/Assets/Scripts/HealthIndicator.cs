using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{

    public Text healthText;
    CharacterStats playerStats;


    void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = playerStats.currentHealth.ToString();
    }
}
