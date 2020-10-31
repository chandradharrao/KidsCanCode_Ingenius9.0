using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            Debug.Log("Goal Triggered,Next level...");
            Gamemanager.instance.currLvl = SceneManager.GetActiveScene().buildIndex+1;
        }
    }
}
