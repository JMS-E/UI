using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItterateOverGamestates : MonoBehaviour
{
    public GameObject[] gameStateArray;
    int steps = 0;
    void Start()
    {

    }

    public void stepOverStates()
    {
        steps++;
        if (steps > gameStateArray.Length - 1)
        {
            steps = 1;
        }
        for(int index = 0; index < gameStateArray.Length; index++)
        {
            gameStateArray[index].SetActive(false);
        }
        gameStateArray[steps].SetActive(true);

    }
    
}
