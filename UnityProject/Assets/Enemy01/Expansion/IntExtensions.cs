using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntExtensions
{
    //‹ô””»’è
    public static bool IsEven(this int value)
    {
        return value % 2 == 0;
    }

    //Šï””»’è
    public static bool IsOdd(this int value)
    {
        return value % 2 != 0;
    }

}
