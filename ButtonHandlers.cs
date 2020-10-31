using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class ButtonHandlers : MonoBehaviour
{
    public Queue EventExe = new Queue();
    public Transform qu;
    public Transform[] wasdTransform;//the transform of children under icons gameobject
    public GameObject[] wasdGO;//the gameobjetcs under icons gameobject
    public Transform wasdParent;//the icon gameobject
    public float verticalOffset = 2.35f;
    float countOfSymbols = 0.5f;
    public Text repeatVal_field;
    public int repeatVal = 1;
    bool insideFor = false;
    public float bracesXOffset;
    float forLoopImgCnt = 0;
    public float incForLoopVal = 0.75f;

    //enum iconName {
    //forward, -> 1
    //right, -> 2
    //left, -> 3
    // yes, -> 4
    //repeat -> 5
    //opening braces ->6
    //closing braces -> 7
    // }

    private void Start()
    {
        //collect the icons in an array
        wasdTransform = wasdParent.GetComponentsInChildren<Transform>();
        wasdGO = new GameObject[wasdTransform.Length];
        for(int i = 0;i<wasdTransform.Length;i++)
        {
            wasdGO[i] = wasdTransform[i].gameObject;
            //Debug.Log(wasdGO[i].name + " , " + i.ToString());
        }
    }

    public void forwardBttn()
    {
        // Debug.Log("forward command was clicked");
        //for (int i = 0; i < repeatVal; i++)
            EventExe.Enqueue('w');
        // Debug.Log("Queue contents are ");
        //foreach (char cmd in EventExe)
        //{
        //Debug.Log(cmd);
        //}
        //GameObject temp = wasdGO[1];
        //temp.SetActive(true);
        //temp.transform.position = new Vector3(qu.position.x,
        //qu.position.y - verticalOffset * countOfSymbols,
        //qu.position.z);
        //Debug.Log(temp.transform.position);
        //Debug.Log("This is the name of the icon " + temp.gameObject.name.ToString());
        //countOfSymbols++;
        displayWASDicons(1);
    }

    public void leftBttn()
    {
        //use shld execute to not allow input during playtime
        //for(int i = 0;i<repeatVal;i++)
            EventExe.Enqueue('a');

        /*GameObject temp = wasdGO[3];
        temp.SetActive(true);
        temp.transform.position = new Vector3(qu.position.x,
            qu.position.y - verticalOffset * countOfSymbols,
            qu.position.z);
        Debug.Log(temp.transform.position);
        countOfSymbols++;*/
        displayWASDicons(3);
    }

    public void StartForLoop()
    {
        string repeatVal_str = repeatVal_field.text.ToString();
        if(Int16.Parse(repeatVal_str) > 0)
        {
            insideFor = true;
        }

        //Debug.Log("The number of tmes user wants to repeat is " + repeatVal_str);
        /*GameObject tempBraces = wasdGO[6];
        tempBraces.SetActive(true);
        tempBraces.transform.position = new Vector3(qu.position.x,
            qu.position.y - verticalOffset * countOfSymbols,
            qu.position.z);

        GameObject tempRepeat = wasdGO[5];
        tempRepeat.SetActive(true);
        tempRepeat.transform.position = new Vector3(tempBraces.transform.position.x + bracesXOffset,
            qu.position.y - verticalOffset * (countOfSymbols+forLoopImgCnt),
            qu.position.z);
        forLoopImgCnt++;*/
        displayWASDicons(6);
        repeatVal = Int16.Parse(repeatVal_str);
        if(repeatVal > 1)
        {
            EventExe.Enqueue('{');
        }
    }

    public void EndForLoop()
    {
        GameObject temp = Instantiate(wasdGO[7]);
        temp.SetActive(true);
        temp.transform.position = new Vector3(qu.position.x,
            qu.position.y - verticalOffset * (countOfSymbols+forLoopImgCnt),
            qu.position.z);
        forLoopImgCnt += incForLoopVal;

        List<char> cmds = new List<char>();
        insideFor = false;
        //int tempCnt = 0;
        int j = 0;
        if(repeatVal > 1)
            EventExe.Enqueue('}');
        var tempArr = EventExe.ToArray();
        //Debug.Log("The Length of temp array is " + tempArr.Length.ToString());
        //now repeat the pattern entered from the { command
        for(int i = 0;i<tempArr.Length;i++)
        {
            j = i;
            if((char)tempArr[i] == '{')//for loop started,hence repeat inside pattern by storing
            {
                j++;
                //Debug.Log("The value of j is " + j.ToString());
                while (j<(tempArr.Length-1) && (char)tempArr[j] != '}')
                {
                    cmds.Add((char)tempArr[j]);//store the commands
                    //tempCnt++;
                    j++;
                }
                //break out of loops
            }
        }
        //now repeat the pattern of elements in queue
        for(int i = 1;i<repeatVal;i++)
        {
            for(int k = 0;k<cmds.Count;k++)
            {
               // Debug.Log("I am repeating " + cmds[k].ToString());
                EventExe.Enqueue(cmds[k]);
            }
        }
        
    }

    public void rightBttn()
    {
        //for (int i = 0; i < repeatVal; i++)
            EventExe.Enqueue('d');
        /*GameObject temp = wasdGO[2];
        temp.SetActive(true);
        temp.transform.position = new Vector3(qu.transform.position.x,
            qu.transform.position.y - verticalOffset * countOfSymbols,
            qu.transform.position.z);
        countOfSymbols++;*/
        displayWASDicons(2);
    }

    public void submitBttn()
    {
        if(!insideFor)
        {
            //Debug.Log("Submitted");
            EventExe.Enqueue('x');
            PlayerMovement.shldExecute = true;

           // Debug.Log("Queue contents are ");
            foreach (char cmd in EventExe)
            {
                //Debug.Log(cmd);
            }
            /*GameObject temp = wasdGO[4];
            temp.SetActive(true);
            temp.transform.position = new Vector3(qu.transform.position.x,
                qu.transform.position.y - verticalOffset *( countOfSymbols),
                qu.transform.position.z);
            countOfSymbols++;*/
            displayWASDicons(4);
        }
    }

    public Queue getCommandsQueue()
    {
        return EventExe;
    }

    void displayWASDicons(int iconType)
    {
        switch(iconType)
        {
            case 1:
                {
                    GameObject temp = Instantiate(wasdGO[iconType]);
                    temp.SetActive(true);
                    temp.transform.position = new Vector3(qu.position.x,
                        qu.position.y - verticalOffset * (countOfSymbols+forLoopImgCnt),
                        qu.position.z);
                    //Debug.Log(temp.transform.position);
                    //Debug.Log("This is the name of the icon " + temp.gameObject.name.ToString());
                    countOfSymbols = countOfSymbols + 1.3f;
                    if(insideFor)
                        forLoopImgCnt += incForLoopVal;
                }
                break;

            case 2:
                {
                    GameObject temp = Instantiate(wasdGO[iconType]);
                    temp.SetActive(true);
                    temp.transform.position = new Vector3(qu.transform.position.x,
                        qu.transform.position.y - verticalOffset *( countOfSymbols+forLoopImgCnt),
                        qu.transform.position.z);
                    //countOfSymbols++;
                    countOfSymbols = countOfSymbols + 1.3f;
                    if (insideFor)
                        forLoopImgCnt += incForLoopVal;
                }
                break;

            case 3:
                {
                    GameObject temp = Instantiate(wasdGO[iconType]);
                    temp.SetActive(true);
                    temp.transform.position = new Vector3(qu.position.x,
                        qu.position.y - verticalOffset * (countOfSymbols+forLoopImgCnt),
                        qu.position.z);
                    //Debug.Log(temp.transform.position);
                    //countOfSymbols++;
                    countOfSymbols = countOfSymbols + 1.3f;
                    if (insideFor)
                        forLoopImgCnt += incForLoopVal;
                }
                break;

            case 4:
                {
                    GameObject temp = Instantiate(wasdGO[iconType]);
                    temp.SetActive(true);
                    temp.transform.position = new Vector3(qu.transform.position.x,
                        qu.transform.position.y - verticalOffset * (countOfSymbols+forLoopImgCnt),
                        qu.transform.position.z);
                    //countOfSymbols++;
                    countOfSymbols = countOfSymbols + 1.3f;
                    if (insideFor)
                        forLoopImgCnt += incForLoopVal;
                }
                break;
            case 6:
                {
                    GameObject tempBraces = Instantiate(wasdGO[iconType]);
                    tempBraces.SetActive(true);
                    tempBraces.transform.position = new Vector3(qu.position.x,
                        qu.position.y - verticalOffset * countOfSymbols,
                        qu.position.z);

                    GameObject tempRepeat = Instantiate(wasdGO[5]);
                    tempRepeat.SetActive(true);
                    tempRepeat.transform.position = new Vector3(tempBraces.transform.position.x + bracesXOffset,
                        qu.position.y - verticalOffset * (countOfSymbols + forLoopImgCnt),
                        qu.position.z);
                    if (insideFor)
                        forLoopImgCnt += incForLoopVal;
                }
                break;
        }
    }

    void deleteCMD()
    {
        //function to delete the command previously entered by mistake
    }
}
