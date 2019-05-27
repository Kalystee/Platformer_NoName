using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    public static bool NullOrEmpty(this string s)
    {
        return s == null || s == "";
    }
}
