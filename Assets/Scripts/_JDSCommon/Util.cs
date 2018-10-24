using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public static class Util
{

    // These help make accidental wrapping less likely.
    public static readonly int bigInt = int.MaxValue - 1000000;
    public static readonly float bigFloat = float.MaxValue - 1000000f;

    //
    public static readonly Vector2 half2 = new Vector2(0.5f, 0.5f);
    public static readonly Vector3 half3 = new Vector3(0.5f, 0.5f, 0.5f);

    //
    public static readonly Color32 white32 = new Color32(255, 255, 255, 255);

    //
    //	public static float FloorMultiple (float x, float m)
    //	{
    //		return Mathf.Floor (x - Util.ModPos (Mathf.Floor (x), m));
    //	}

    public static int StepInt(int x, int m)
    {
        return x - Util.ModPos(x, m);
    }

    public static float Step(float x, float stepSize)
    {
        return Mathf.Floor(x / stepSize) * stepSize;
    }

    public static int Abs(int x)
    {
        if (x < 0)
        {
            return 0 - x;
        }
        else
        {
            return x;
        }
    }

    public static float Abs(float x)
    {
        if (x < 0f)
        {
            return 0f - x;
        }
        else
        {
            return x;
        }
    }

    public static int Sign(float x)
    {
        if (x < 0)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public static float Repeat(float x, float m)
    {
        x = x % m;
        return (x < 0) ? (m < 0 ? x - m : x + m) : x;
    }

    public static int Repeat(int x, int m)
    {
        x = x % m;
        return (x < 0) ? (m < 0 ? x - m : x + m) : x;
    }

    public static int Pow(int num, int exp)
    {
        int result = 1;
        while (exp > 0)
        {
            if (exp % 2 == 1)
            {
                result *= num;
            }
            exp >>= 1;
            num *= num;
        }

        return result;
    }

    public static float Pow(float num, int exp)
    {
        float result = 1.0f;
        while (exp > 0)
        {
            if (exp % 2 == 1)
            {
                result *= num;
            }
            exp >>= 1;
            num *= num;
        }

        return result;
    }

    public static Vector2 Vec3to2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 Vec2to3(Vector2 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector2 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static bool Approximately(float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    public static bool Approximately(Vector2 a, Vector2 b)
    {
        return Approximately(a.x, b.x)
        && Approximately(a.y, b.y);
    }

    public static bool Approximately(Vector3 a, Vector3 b)
    {
        return Approximately(a.x, b.x)
        && Approximately(a.y, b.y)
        && Approximately(a.z, b.z);
    }

    public static bool IsInRange(this int i, int min, int max, bool includeMax = true)
    {
        if (min <= max)
        {
            return (i >= min && i < max) || (includeMax && i == max);
        }
        else
        {
            return (i >= max && i < min) || (includeMax && i == min);
        }
    }

    public static bool IsInRange(this float i, float min, float max, bool includeMax = true)
    {
        if (min <= max)
        {
            return (i >= min && i < max) || (includeMax && i == max);
        }
        else
        {
            return (i >= max && i < min) || (includeMax && i == min);
        }
    }

    public static bool Empty(string s)
    {
        return s == null || s.Length <= 0;
    }

    public static bool IsEmpty(this string s)
    {
        return Empty(s);
    }

    public static bool EmptyAfterTrim(string s)
    {
        return s == null || s.Trim().Length <= 0;
    }

    public static bool IsEmptyAfterTrim(this string s)
    {
        return EmptyAfterTrim(s);
    }

    public static string Readable(this int i)
    {
        return i.ToString("N0");
    }

    public static string Readable(this uint i)
    {
        return i.ToString("N0");
    }

    public static string Readable(this long i)
    {
        return i.ToString("N0");
    }

    public static string Readable(this float f, int decimalPlaces)
    {
        return f.ToString(Concat("N", decimalPlaces));
    }

    public static float RoundedDistance(Vector3 a, Vector3 b)
    {
        return RoundedDistance(a.x, a.y, a.z, b.x, b.y, b.z);
    }

    public static float RoundedDistance(float aX, float aY, float aZ, float bX, float bY, float bZ)
    {
        // Results should be reasonably similar to Distance(), without using Sqrt.

        float x = Util.Abs(aX - bX);
        float y = Util.Abs(aY - bY);
        float z = Util.Abs(aZ - bZ);

        float max = x;
        if (y > max)
        {
            max = y;
        }
        if (z > max)
        {
            max = z;
        }

        float min = x;
        if (y < min)
        {
            min = y;
        }
        if (z < min)
        {
            min = z;
        }

        float mid = x + y + z - max - min;

        return max + (mid * 0.414f) + (min * 0.318f);
    }

    public static float TaxiDistance(Vector2 a, Vector2 b)
    {
        return Util.Abs(a.x - b.x) + Util.Abs(a.y - b.y);
    }

    public static float ActualDistance(Vector2 a, Vector2 b)
    {
        return Mathf.Sqrt(((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y)));
    }

    public static float SquareDistance(Vector2 a, Vector2 b)
    {
        return Mathf.Max(Util.Abs(a.x - b.x), Util.Abs(a.y - b.y));
    }

    public static T RandomEnumValue<T>(bool ignoreFirstElement)
    {
        T[] values = (T[])System.Enum.GetValues(typeof(T));
        return values[Random.Range(ignoreFirstElement ? 1 : 0, values.Length)];
    }

    public static int EnumLength<T>()
    {
        return System.Enum.GetValues(typeof(T)).Length;
    }

    // This interleaves positive and negative values.
    // 0, -1, 1, -2, 2, -3, 3...
    public static int DimIndex(int x)
    {
        if (x < 0)
        {
            return ((-1 - x) << 1) | 1;
        }
        else
        {
            return x << 1;
        }
    }

    public static float Atan2(Vector2 v)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(v.x, v.y);
    }

    public static float ModPos(float x, float m)
    {
        x = x % m;
        return (x < 0) ? (x + m) : x;
    }

    public static int ModPos(int x, int m)
    {
        x = x % m;
        return (x < 0) ? (x + m) : x;
    }

    public static float ModDiff(float x, float m)
    {
        float mod = ModPos(x, m);
        return Mathf.Min(mod, m - mod);
    }

    public static Color32 Average(params Color32[] colors)
    {
        float r = 0;
        float g = 0;
        float b = 0;
        float a = 0;

        for (int i = 0; i < colors.Length; i++)
        {
            Color32 c = colors[i];
            r += c.r;
            g += c.g;
            b += c.b;
            a += c.a;
        }

        float div = 1f / colors.Length;

        r *= div;
        g *= div;
        b *= div;
        a *= div;

        return new Color32((byte)r, (byte)g, (byte)b, (byte)a);
    }

    public static Color32 ColorInt(int r, int g, int b, int a)
    {
        return new Color32(
            (byte)(r < 0 ? 0 : (r > 255 ? 255 : r)),
            (byte)(g < 0 ? 0 : (g > 255 ? 255 : g)),
            (byte)(b < 0 ? 0 : (b > 255 ? 255 : b)),
            (byte)(a < 0 ? 0 : (a > 255 ? 255 : a))
        );
    }

    public static Color32 GrayScale(int val)
    {
        byte v = (byte)(val < 0 ? 0 : (val > 255 ? 255 : val));
        return new Color32(v, v, v, 255);
    }

    public static Color32 GrayScale(int val, byte alpha)
    {
        byte v = (byte)(val < 0 ? 0 : (val > 255 ? 255 : val));
        return new Color32(v, v, v, (byte)alpha);
    }

    public static Color32 GrayScale(float val)
    {
        return GrayScale(Mathf.FloorToInt(val));
    }

    //
    static StringBuilder stringBuilder = new StringBuilder();

    public static string Concat(params object[] parts)
    {
        if (parts.Length <= 0)
        {
            return string.Empty;
        }

        stringBuilder.Length = 0;
        for (int i = 0; i < parts.Length; i++)
        {
            object part = parts[i];
            stringBuilder.Append(part.ToString());
        }

        return stringBuilder.ToString();
    }

    public static string ConcatStr(params string[] parts)
    {
        if (parts.Length <= 0)
        {
            return string.Empty;
        }

        stringBuilder.Length = 0;
        for (int i = 0; i < parts.Length; i++)
        {
            string part = parts[i];
            stringBuilder.Append(part);
        }

        return stringBuilder.ToString();
    }

    //
    public static void DeleteAllFiles(string dirPath)
    {
        DirectoryInfo dir = new DirectoryInfo(dirPath);

        FileInfo[] files = dir.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            files[i].Delete();
        }

        DirectoryInfo[] subDirs = dir.GetDirectories();
        for (int i = 0; i < subDirs.Length; i++)
        {
            subDirs[i].Delete(true);
        }
    }

    public static byte ToByte(this int i)
    {
        return (i < 0 ? (byte)0 : (i > 255 ? (byte)255 : (byte)i));
    }

    public static byte ToByte(this float f)
    {
        return ToByte(Mathf.FloorToInt(f));
    }

    public static T FindInParents<T>(GameObject gameObject) where T : Component
    {
        if (gameObject == null)
        {
            return null;
        }

        Transform parent = gameObject.transform.parent;
        Component comp = gameObject.GetComponent<T>();

        while (parent != null && comp == null)
        {
            comp = parent.gameObject.GetComponent<T>();
            parent = parent.parent;
        }

        return (T)comp;
    }

    public static bool Color32Equal(Color32 c0, Color32 c1)
    {
        return (c0.r == c1.r) && (c0.g == c1.g) && (c0.b == c1.b) && (c0.a == c1.a);
    }

    public static bool Contains<T>(this T[] array, T val)
    {
        if (array == null || val == null)
        {
            return false;
        }

        for (int i = 0; i < array.Length; i++)
        {
            if (val.Equals(array[i]))
            {
                return true;
            }
        }

        return false;
    }

    public static T LastElement<T>(this T[] array)
    {
        if (array == null || array.Length <= 0)
        {
            return default(T);
        }
        return array[array.Length - 1];
    }

    public static T LastElement<T>(this List<T> list)
    {
        if (list == null || list.Count <= 0)
        {
            return default(T);
        }
        return list[list.Count - 1];
    }

    public static T RandomElement<T>(this T[] array)
    {
        if (array == null || array.Length <= 0)
        {
            return default(T);
        }
        return array[Random.Range(0, array.Length)];
    }

    public static T RandomElement<T>(this List<T> list)
    {
        if (list == null || list.Count <= 0)
        {
            return default(T);
        }
        return list[Random.Range(0, list.Count)];
    }

    public static void DestroyAllChildren(this GameObject root)
    {
        root.transform.DestroyAllChildren();
    }

    public static void DestroyAllChildren(this Transform root)
    {
        int childCount = root.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(root.GetChild(i).gameObject);
        }
    }

    public static RectTransform GetRectTransform(this GameObject gameObject)
    {
        return gameObject.GetComponent<RectTransform>();
    }

    public static RectTransform GetRectTransform(this MonoBehaviour behaviour)
    {
        return behaviour.GetComponent<RectTransform>();
    }

    public static List<T> Copy<T>(this List<T> list)
    {
        List<T> result = new List<T>();

        for (int i = 0; i < list.Count; i++)
        {
            result.Add(list[i]);
        }

        return result;
    }

    public static int CompareMagnitude(Vector2 v, float x)
    {
        float result = v.sqrMagnitude - (x * x);

        if (result < 0)
        {
            return -1;
        }
        if (result > 0)
        {
            return 1;
        }

        return 0;
    }

    public static int CompareMagnitude(Vector2Int c, float x)
    {
        float result = (c.x * c.x + c.y * c.y) - (x * x);

        if (result < 0)
        {
            return -1;
        }
        if (result > 0)
        {
            return 1;
        }

        return 0;
    }

    public static float Left(this Vector4 vec)
    {
        return vec.x;
    }

    public static float Top(this Vector4 vec)
    {
        return vec.y;
    }

    public static float Right(this Vector4 vec)
    {
        return vec.z;
    }

    public static float Bottom(this Vector4 vec)
    {
        return vec.w;
    }

    public static bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    public static float RandomNormal(float min = 0f, float max = 1f, int iterations = 12)
    {
        float sum = 0f;
        for (int i = 0; i < iterations; i++)
        {
            sum += Random.value;
        }

        float avg = sum / (float)iterations;

        float range = max - min;
        float result = (avg * range) + min;

        return result;
    }

    public static int RandomNormalInt(float min = 0, float max = 1, int iterations = 12)
    {
        return Mathf.RoundToInt(RandomNormal(min, max, iterations));
    }

    public static T Get<T>(this Component a) where T : Component
    {
        //        if(a.GetComponent<T>() == null)
        //        {
        //            Debug.LogError("Can't find " + typeof(T).Name + " on " + a.name);
        //        }

        return a.GetComponent<T>();
    }

    public static T Get<T>(this GameObject a) where T : Component
    {
        //        if(a.GetComponent<T>() == null)
        //        {
        //            Debug.LogError("Can't find " + typeof(T).Name + " on " + a.name);
        //        }

        return a.GetComponent<T>();
    }

    public static bool Has<T>(this Component a) where T : Component
    {
        return (a.GetComponent<T>() != null);
    }

    public static bool Has<T>(this GameObject a) where T : Component
    {
        return (a.GetComponent<T>() != null);
    }

    public static GameObject CreateGameObject(Transform parent, string name)
    {
        GameObject result = new GameObject();
        result.name = name;
        result.transform.SetParent(parent, false);
        return result;
    }

    public static T Clone<T>(T prefab, Transform parent) where T : Object
    {
        return (T)Object.Instantiate(prefab, parent);
    }

    public static T[] ConcatArrays<T>(params T[][] arrays)
    {
        List<T> result = new List<T>();

        foreach (T[] arr in arrays)
        {
            result.AddRange(arr);
        }

        return result.ToArray();
    }

    public static int TaxiDistance(Vector2Int a, Vector2Int b)
    {
        return Util.Abs(a.x - b.x) + Util.Abs(a.y - b.y);
    }

    public static int SquareDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Max(Util.Abs(a.x - b.x), Util.Abs(a.y - b.y));
    }

}
