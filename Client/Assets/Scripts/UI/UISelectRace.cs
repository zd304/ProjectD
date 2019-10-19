using System;
using System.Collections.Generic;
using UnityEngine;

public class UISelectRace : MonoBehaviour
{
    private const int raceCount = 3;
    private const int defaultRace = 2;

    public Transform[] raceTranforms;
    public GameObject[] raceBGs;

    private int raceIndex = defaultRace;

    public void OnClickLeftPage()
    {
        if (raceIndex > 1)
        {
            --raceIndex;
        }
        for (int i = 0; i < raceTranforms.Length; ++i)
        {
            Transform t = raceTranforms[i];
            GameObject bg = raceBGs[i];
            if (i == (raceIndex - 1))
            {
                t.localScale = Vector3.one;
                bg.SetActive(true);
            }
            else
            {
                t.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                bg.SetActive(false);
            }
        }
    }

    public void OnClickRightPage()
    {
        if (raceIndex < raceCount)
        {
            ++raceIndex;
        }
        for (int i = 0; i < raceTranforms.Length; ++i)
        {
            Transform t = raceTranforms[i];
            GameObject bg = raceBGs[i];
            if (i == (raceIndex - 1))
            {
                t.localScale = Vector3.one;
                bg.SetActive(true);
            }
            else
            {
                t.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                bg.SetActive(false);
            }
        }
    }
}