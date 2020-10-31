using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public int currLvl = 1;
    public static Gamemanager instance;

   private void Start()
    {
        currLvl = SceneManager.GetActiveScene().buildIndex;
        //temp assignment,change it soon in the final build
        //currLvl = 3;
        instance = this;
        Debug.Log("We are level " + currLvl.ToString() + " currently");
    }

    private void Update()
    {
        if(PlayerMovement.isLevelFailed == true)
        {
            PlayerMovement.shldExecute = false;
            PlayerMovement.isLevelFailed = false;
            SceneManager.LoadScene(currLvl);
        }
    }
}
