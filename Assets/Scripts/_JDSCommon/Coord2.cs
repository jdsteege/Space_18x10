using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Coord2 : System.IEquatable<Coord2>
{

    public static readonly Coord2 zero = new Coord2(0, 0);
    public static readonly Coord2 unit = new Coord2(1, 1);
    public static readonly Coord2 one = new Coord2(1, 1);
    public static readonly Coord2 two = new Coord2(2, 2);
    public static readonly Coord2 unitNeg = new Coord2(-1, -1);
    public static readonly Coord2 left = new Coord2(-1, 0);
    public static readonly Coord2 right = new Coord2(1, 0);
    public static readonly Coord2 bottom = new Coord2(0, -1);
    public static readonly Coord2 down = new Coord2(0, -1);
    public static readonly Coord2 top = new Coord2(0, 1);
    public static readonly Coord2 up = new Coord2(0, 1);

    //
    public readonly int x;
    public readonly int y;

    // Constructors
    public Coord2(int xVal, int yVal)
    {
        x = xVal;
        y = yVal;
    }

    public Coord2(Coord2 c) : this(c.x, c.y)
    {
    }

    public Coord2(float xVal, float yVal) : this(Mathf.FloorToInt(xVal), Mathf.FloorToInt(yVal))
    {
    }

    public Coord2(Vector2 v) : this(v.x, v.y)
    {
    }

    public Coord2(Vector3 v) : this(v.x, v.y)
    {
    }

    //
    public static implicit operator Vector2(Coord2 c)
    {
        return new Vector2(c.x, c.y);
    }

    public static implicit operator Vector2Int(Coord2 c)
    {
        return new Vector2Int(c.x, c.y);
    }

    public static implicit operator Coord2(Vector2Int c)
    {
        return new Coord2(c.x, c.y);
    }

    public static implicit operator Coord2(Coord3 c)
    {
        return new Coord2(c.x, c.y);
    }

    //
    public Coord2 Opposite()
    {
        return new Coord2(-x, -y);
    }

    // Object overrides
    public override int GetHashCode()
    {
        return (y << 16) | (x & 0xFFFF);
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(Coord2))
        {
            return false;
        }
        return (this == (Coord2)obj);
    }

    public override string ToString()
    {
        return Util.ConcatStr("(", x.ToString(), ",", y.ToString(), ")");
    }

    public static Coord2 FromString(string s)
    {
        string[] parts = s.Split('(', ',');
        return new Coord2(int.Parse(parts[0]),
            int.Parse(parts[1]));
    }

    public bool Equals(Coord2 other)
    {
        return this.x == other.x && this.y == other.y;
    }

    //
    public bool Equals(int x, int y)
    {
        return this.x == x && this.y == y;
    }

    public static bool Equals(Coord2 c, int x, int y)
    {
        return c.x == x && c.y == y;
    }

    //
    public static bool operator ==(Coord2 a, Coord2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Coord2 a, Coord2 b)
    {
        return a.x != b.x || a.y != b.y;
    }

    //

    public static Coord2 operator +(Coord2 a, Coord2 b)
    {
        return new Coord2(a.x + b.x, a.y + b.y);
    }

    public static Coord2 operator +(Coord2 a, Vector2 b)
    {
        return new Coord2(
            (float)a.x + b.x,
            (float)a.y + b.y);
    }

    public static Coord2 operator -(Coord2 a, Coord2 b)
    {
        return new Coord2(a.x - b.x, a.y - b.y);
    }

    public static Coord2 operator -(Coord2 a, Vector2 b)
    {
        return new Coord2(
            (float)a.x - b.x,
            (float)a.y - b.y);
    }

    public static Coord2 operator *(Coord2 a, int i)
    {
        return new Coord2((float)a.x * i, (float)a.y * i);
    }

    public static Coord2 operator *(Coord2 a, float f)
    {
        return new Coord2(Mathf.FloorToInt(a.x * f),
            Mathf.FloorToInt(a.y * f));
    }

    //
    public static float Distance(Coord2 a, Coord2 b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
    }

    public static int TaxiDistance(Coord2 a, Coord2 b)
    {
        return Util.Abs(a.x - b.x) + Util.Abs(a.y - b.y);
    }

    public static int SquareDistance(Coord2 a, Coord2 b)
    {
        return Mathf.Max(Util.Abs(a.x - b.x), Util.Abs(a.y - b.y));
    }

    public static Coord2 Nearest(Vector2 v)
    {
        return new Coord2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public bool IsInRange(Coord2 min, Coord2 max)
    {
        return (x >= min.x && x <= max.x && y >= min.y && y <= max.y);
    }

    //
    public static Coord2 Reverse(Coord2 a)
    {
        return new Coord2(-a.x, -a.y);
    }

    public static Coord2 RotateLeft90(Coord2 a)
    {
        return new Coord2(-a.y, a.x);
    }

    public static Coord2 RotateRight90(Coord2 a)
    {
        return new Coord2(a.y, -a.x);
    }

    //
    public static Coord2 Normalize(Coord2 a, bool orthogonalOnly)
    {
        int resultX = 0;
        int resultY = 0;

        if (a.x < 0)
        {
            resultX = -1;
        }
        else if (a.x > 0)
        {
            resultX = 1;
        }

        if (a.y < 0)
        {
            resultY = -1;
        }
        else if (a.y > 0)
        {
            resultY = 1;
        }

        if (orthogonalOnly)
        {
            if (Util.Abs(a.x) <= Util.Abs(a.y))
            {
                resultX = 0;
            }
            else
            {
                resultY = 0;
            }
        }

        if (resultX == 0 && resultY == 0)
        {
            resultX = 1;
        }

        return new Coord2(resultX, resultY);
    }

}
