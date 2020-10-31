using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public static bool shldExecute = false;
    public GameObject[] platforms;
    public GameObject[] obstacles;
    Vector3 initialPos;
    public float offset = 0.075f;
    bool next = true;
    int boolBttnCount = 0;
    bool isAllBttnsPressed = false;
    bool obstaclesDisabled = false;
    public float contactObstaclesMinDist = 1.15f;
    public static bool isLevelFailed = false;//use this also to detect boundary consition lossses like into water or out of screen
    public float platformCheckOffset = 0.5f;
    //Animator anim;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        platforms = GameObject.FindGameObjectsWithTag("platform");
        obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        initialPos = transform.position;
    }

    private void Update()
    {
        if(shldExecute)
        {
            Queue commands = GameObject.FindGameObjectWithTag("bttnManager").GetComponent<ButtonHandlers>().getCommandsQueue();
            while(commands.Count>0 && next)
            {
                char command = 'x';
                if(commands.Count!=0)
                {
                    command = (char)commands.Dequeue();
                }

                switch (command)
                {
                    case 'w':
                        {
                            StartCoroutine(moveMe('w',3f));
                            next = false;//since couroutines are non blocking,hence the below codes would be executed when the coorotine is functioning,hence we need to wait until the coorotine finishes
                            break;
                        }

                    case 'a':
                        {
                            StartCoroutine(moveMe('a', 3f));
                            next = false; ;
                            break;
                        }

                    case 'd':
                        {
                            StartCoroutine(moveMe('d', 3f));
                            next = false;
                            break;
                        }

                    case 'x':
                        shldExecute = false;
                        next = false;
                        break;

                    default:
                        break;
                }
            }
        }

        if(Gamemanager.instance.currLvl ==2)
        {
            if (isAllBttnsPressed && !obstaclesDisabled)
            {
                //Debug.Log("All buttons have been triggered,now clear the obstacles....");
                var toDsiable = GameObject.FindGameObjectsWithTag("obstacle");
                for (int j = 0; j < toDsiable.Length; j++)
                {
                    toDsiable[j].SetActive(false);
                }
                obstaclesDisabled = true;
            }
            else
            {
                if (!obstaclesDisabled)
                {
                    if(obstacles.Length > 0)
                    {
                        //Debug.Log("Obstacles are present at dist ");
                        for (int i = 0; i < obstacles.Length; i++)
                        {
                            //Debug.Log(obstacles[i].transform.position);
                            //if (transform.position.z == obstacles[i].transform.position.z 
                            //&& transform.position.x == obstacles[i].transform.position.x)
                            //Debug.Log(Vector3.Distance(transform.position, obstacles[i].transform.position));
                            //Debug.Log(" from the player");
                            if (Vector3.Distance(transform.position, obstacles[i].transform.position)
                                <= contactObstaclesMinDist)
                            {
                                Debug.Log("END OF CODING");
                                isLevelFailed = true;
                            }
                        }
                    }
                }
            }
        }
        else if(Gamemanager.instance.currLvl == 1)
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                //if (transform.position.z == obstacles[i].transform.position.z 
                //&& transform.position.x == obstacles[i].transform.position.x)
                if (Vector3.Distance(transform.position, obstacles[i].transform.position)
                    <= contactObstaclesMinDist)
                {
                    Debug.Log("END OF CODING");
                    isLevelFailed = true;
                }
            }
        }
        else if (Gamemanager.instance.currLvl == 3)
        {

            if(obstacles.Length > 0)
            {
                for (int i = 0; i < obstacles.Length; i++)
                {
                    //if (transform.position.z == obstacles[i].transform.position.z 
                    //&& transform.position.x == obstacles[i].transform.position.x)
                    if (Vector3.Distance(transform.position, obstacles[i].transform.position)
                        <= contactObstaclesMinDist)
                    {
                        Debug.Log("END OF CODING");
                        isLevelFailed = true;
                    }
                }
            }
        }
    }

    IEnumerator moveMe(char dir,float time)
    {
        //anim.SetBool("isJump", true);
        Vector3 startingPos = transform.position;
        Vector3 finalPos = transform.position;
        if(dir == 'w')
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x,
                0f, transform.rotation.z);
            finalPos = new Vector3(transform.position.x,
                            transform.position.y,
                            transform.position.z + 5f);
        }
        else if(dir == 'a')
        {
            //try to smooth out rotation too
            transform.rotation = Quaternion.Euler(transform.rotation.x,
                                - 90f,
                                transform.rotation.z);
            finalPos = new Vector3(transform.position.x - 5f,
                transform.position.y, transform.position.z);
        }
        else if (dir == 'd')
        {
            //Debug.Log("Moving to the right...");
            //try to smooth out rotation too
            transform.rotation = Quaternion.Euler(transform.rotation.x,
                                90f,
                                transform.rotation.z);
            finalPos = new Vector3(transform.position.x + 5f,
                transform.position.y, transform.position.z);
        }

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float currDist = Vector3.Distance(transform.position, finalPos);
            if (currDist > offset)
                transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            else
                transform.position = finalPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //anim.SetBool("isJump", false);
        Debug.Log("The players pos is " + transform.position.ToString());
        bool onASinglePlatform = false;
        for(int i = 0;i<platforms.Length;i++)
        {
            //if((platforms[i].transform.position.x == gameObject.transform.position.x + platformCheckOffset))//since bird pauses before its specified location
            if((platforms[i].transform.position.x - platformCheckOffset <= gameObject.transform.position.x) ||
                (platforms[i].transform.position.x + platformCheckOffset >= gameObject.transform.position.x ))//check for range
            {
                onASinglePlatform = true;
            }
        }
        if(!onASinglePlatform)
        {
            Debug.Log("YOU LOOSE....CODE AGAIN...");
            isLevelFailed = true;
        }
        next = true;
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Inside ontrigger");
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("bool"))
        {
            if (Gamemanager.instance.currLvl == 2)
            {
                if (boolBttnCount == 1)
                {
                    isAllBttnsPressed = true;
                    //remove obstacle
                    Array.Clear(obstacles, 0, obstacles.Length);
                }
                else
                {
                    //Debug.Log("Bttn pressed is " + boolBttnCount.ToString());
                    boolBttnCount++;
                }
            }
        }

        if(other.gameObject.CompareTag("barriers"))
        {
            Debug.Log("Collided with a barrier..");
            isLevelFailed = true;
        }
    }
    //make sinking animation using lerp in unity
}
