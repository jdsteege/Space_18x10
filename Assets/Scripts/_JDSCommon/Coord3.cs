using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Coord3 : System.IEquatable<Coord3>
{

    public static readonly Coord3 zero = new Coord3(0, 0, 0);
    public static readonly Coord3 unit = new Coord3(1, 1, 1);
    public static readonly Coord3 one = new Coord3(1, 1, 1);
    public static readonly Coord3 two = new Coord3(2, 2, 2);
    public static readonly Coord3 unitNeg = new Coord3(-1, -1, -1);
    public static readonly Coord3 left = new Coord3(-1, 0, 0);
    public static readonly Coord3 right = new Coord3(1, 0, 0);
    public static readonly Coord3 bottom = new Coord3(0, -1, 0);
    public static readonly Coord3 down = new Coord3(0, -1, 0);
    public static readonly Coord3 top = new Coord3(0, 1, 0);
    public static readonly Coord3 up = new Coord3(0, 1, 0);
    public static readonly Coord3 front = new Coord3(0, 0, 1);
    public static readonly Coord3 back = new Coord3(0, 0, -1);

    //
    public readonly int x;
    public readonly int y;
    public readonly int z;

    // Constructors
    public Coord3(int xVal, int yVal, int zVal)
    {
        x = xVal;
        y = yVal;
        z = zVal;
    }

    public Coord3(Coord3 c) : this(c.x, c.y, c.z)
    {
    }

    public Coord3(float xVal, float yVal, float zVal) : this(Mathf.FloorToInt(xVal), Mathf.FloorToInt(yVal), Mathf.FloorToInt(zVal))
    {
    }

    public Coord3(Vector3 v) : this(v.x, v.y, v.z)
    {
    }

    //
    public static implicit operator Vector3(Coord3 c)
    {
        return new Vector3(c.x, c.y, c.z);
    }

    public static implicit operator Vector3Int(Coord3 c)
    {
        return new Vector3Int(c.x, c.y, c.z);
    }

    public static implicit operator Coord3(Vector3Int c)
    {
        return new Coord3(c.x, c.y, c.z);
    }

    //
    public Coord3 Opposite()
    {
        return new Coord3(-x, -y, -z);
    }

    // Object overrides
    public override int GetHashCode()
    {
        return ((z & 0xFF) << 16) | ((y & 0xFF) << 8) | (x & 0xFF);
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(Coord3))
        {
            return false;
        }
        return (this == (Coord3)obj);
    }

    public override string ToString()
    {
        return Util.ConcatStr("(", x.ToString(), ",", y.ToString(), ",", z.ToString(), ")");
    }

    public static Coord3 FromString(string s)
    {
        string[] parts = s.Split('(', ',');
        return new Coord3(int.Parse(parts[0]),
            int.Parse(parts[1]),
            int.Parse(parts[2]));
    }

    public bool Equals(Coord3 other)
    {
        return this.x == other.x && this.y == other.y && this.z == other.z;
    }

    //
    public bool Equals(int x, int y, int z)
    {
        return this.x == x && this.y == y && this.z == z;
    }

    public static bool Equals(Coord3 c, int x, int y, int z)
    {
        return c.x == x && c.y == y && c.z == z;
    }

    //
    public static bool operator ==(Coord3 a, Coord3 b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(Coord3 a, Coord3 b)
    {
        return a.x != b.x || a.y != b.y || a.z != b.z;
    }

    //

    public static Coord3 operator +(Coord3 a, Coord3 b)
    {
        return new Coord3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Coord3 operator +(Coord3 a, Vector3 b)
    {
        return new Coord3(
            (float)a.x + b.x,
            (float)a.y + b.y,
            (float)a.z + b.z);
    }

    public static Coord3 operator -(Coord3 a, Coord3 b)
    {
        return new Coord3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Coord3 operator -(Coord3 a, Vector3 b)
    {
        return new Coord3(
            (float)a.x - b.x,
            (float)a.y - b.y,
            (float)a.z - b.z);
    }

    public static Coord3 operator *(Coord3 a, int i)
    {
        return new Coord3((float)a.x * i, (float)a.y * i, (float)a.z * i);
    }

    public static Coord3 operator *(Coord3 a, float f)
    {
        return new Coord3(Mathf.FloorToInt(a.x * f),
            Mathf.FloorToInt(a.y * f),
            Mathf.FloorToInt(a.z * f));
    }

    //
    public static float Distance(Coord3 a, Coord3 b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z));
    }

    public static int TaxiDistance(Coord3 a, Coord3 b)
    {
        return Util.Abs(a.x - b.x) + Util.Abs(a.y - b.y) + Util.Abs(a.z - b.z);
    }

    public static int SquareDistance(Coord3 a, Coord3 b)
    {
        return Mathf.Max(Util.Abs(a.x - b.x), Util.Abs(a.y - b.y), Util.Abs(a.z - b.z));
    }

    public static Coord3 Nearest(Vector3 v)
    {
        return new Coord3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
    }

    public bool IsInRange(Coord3 min, Coord3 max)
    {
        return (x >= min.x && x <= max.x && y >= min.y && y <= max.y && z >= min.z && z <= max.z);
    }

    //
    public static Coord3 Reverse(Coord3 a)
    {
        return new Coord3(-a.x, -a.y, -a.z);
    }

    //	public static Coord3 RotateLeft90(Coord3 a)
    //	{
    //		return new Coord3(-a.y, a.x);
    //	}
    //
    //	public static Coord3 RotateRight90(Coord3 a)
    //	{
    //		return new Coord3(a.y, -a.x);
    //	}

    //
    public static Coord3 Normalize(Coord3 a, bool orthogonalOnly)
    {
        int resultX = 0;
        int resultY = 0;
        int resultZ = 0;

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

        if (a.z < 0)
        {
            resultZ = -1;
        }
        else if (a.z > 0)
        {
            resultZ = 1;
        }

        if (orthogonalOnly)
        {
            if (Util.Abs(a.x) >= Util.Abs(a.y) && Util.Abs(a.x) >= Util.Abs(a.z))
            {
                resultY = 0;
                resultZ = 0;

            }
            else if (Util.Abs(a.y) >= Util.Abs(a.x) && Util.Abs(a.y) >= Util.Abs(a.z))
            {
                resultX = 0;
                resultZ = 0;

            }
            else
            {
                resultX = 0;
                resultY = 0;
            }
        }

        if (resultX == 0 && resultY == 0 && resultZ == 0)
        {
            resultX = 1;
        }

        return new Coord3(resultX, resultY, resultZ);
    }

}
