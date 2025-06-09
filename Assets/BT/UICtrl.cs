using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    public Text timerText;
    public Text playerStatus;
    public Text agentStatus;

    private float timeElapsed;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        timerText.text = $"Timer: {timeElapsed:F2}";

        if (Input.GetKeyDown(KeyCode.L)) // Simulate player finish
        {
            playerStatus.text = "Player Finished: Yes";
            Debug.Log("Player reached exit in " + timeElapsed + " seconds");

        }

        if (Input.GetKeyDown(KeyCode.K)) // Simulate agent finish
        {
            agentStatus.text = "Agent Finished: Yes";
            Debug.Log("Agent reached exit in " + timeElapsed + " seconds");

        }
    }

}

