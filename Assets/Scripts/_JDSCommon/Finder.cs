using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Finder
{

    //
    protected List<GameObject> resultList;

    //
    protected Finder()
    {
        resultList = new List<GameObject>();
    }

    public List<GameObject> Results()
    {
        return resultList;
    }

    //
    public static Finder AllGameObjects()
    {
        Finder finder = new Finder();

        GameObject[] foundArray = Object.FindObjectsOfType<GameObject>();
        finder.resultList.AddRange(foundArray);

        return finder;
    }

    public static Finder AllWith<T>() where T : Component
    {
        Finder finder = new Finder();

        T[] foundArray = Object.FindObjectsOfType<T>();

        foreach(T found in foundArray)
        {
            finder.resultList.Add(found.gameObject);
        }

        return finder;
    }

    public static T SingletonOf<T>() where T : Component
    {
        return Object.FindObjectOfType<T>();
    }

    //
    public delegate bool FinderTest(GameObject subject);

    public Finder PassesTest(FinderTest test)
    {
        System.Func<GameObject, bool> testFunc = (GameObject subject) =>
        {
            return test(subject);
        };
        
        resultList = resultList.Where(testFunc).ToList();

        return this;
    }

    public Finder With<T>() where T : Component
    {
        FinderTest test = (GameObject subject) =>
        {
            return (subject.GetComponent<T>() != null);
        };

        return PassesTest(test);
    }

    public Finder Without<T>() where T : Component
    {
        FinderTest test = (GameObject subject) =>
        {
            return (subject.GetComponent<T>() == null);
        };

        return PassesTest(test);
    }

}
