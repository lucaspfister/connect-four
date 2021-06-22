using System.Collections.Generic;
using UnityEngine;

public static class AI
{
    public static int GetRandomValue(List<int> lstValues)
    {
        int index = Random.Range(0, lstValues.Count);
        return lstValues[index];
    }
}
