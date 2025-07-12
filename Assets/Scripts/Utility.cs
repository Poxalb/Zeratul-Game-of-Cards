using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class Utility 
{
    public static void Shuffle<T>(List<T> list) // Shuffles a list in place using the Fisher-Yates algorithm
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
