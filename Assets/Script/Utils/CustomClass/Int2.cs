using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable, DataContract]
public class Int2
{
    [DataMember]
    public int x;
    [DataMember]
    public int y;

    public Int2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int Min
    {
        get
        {
            return Mathf.Min(x, y);
        }
    }

    public int Max
    { 
        get
        {
            return Mathf.Max(x, y);
        }
    }

    public static int Distance(Int2 a, Int2 b)
    {
        return Mathf.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public static Int2 operator +(Int2 a, Int2 b)
    {
        return new Int2(a.x + b.x, a.y + b.y);
    }

    public static Int2 operator -(Int2 a, Int2 b)
    {
        return new Int2(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(Int2 a, Int2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Int2 a, Int2 b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public static implicit operator Int2(Vector2 v)
    {
        return new Int2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static implicit operator Int2(Vector3 v)
    {
        return new Int2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static implicit operator Vector2(Int2 i)
    {
        return new Vector2(i.x, i.y);
    }

    public static implicit operator Vector3(Int2 i)
    {
        return new Vector3(i.x, i.y);
    }

    public static implicit operator Int2(string s)
    {
        string[] values = Regex.Split(s, ",");
        return new Int2(int.Parse(values[0]), int.Parse(values[1]));
    }

    public override bool Equals(object a)
    {
        if (a is Int2)
        {
            Int2 b = (Int2)a;
            return (b.x == x && b.y == y);
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hashCode = 2113303842;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(this);
        hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(this);
        return hashCode;
    }
}
