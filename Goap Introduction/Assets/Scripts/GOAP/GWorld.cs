using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();

    private static Queue<GameObject> patientsQueue;
    private static Queue<GameObject> cubicles;

    private static WorldStates world;

    static GWorld()
    {
        world = new WorldStates();
        patientsQueue = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach(GameObject c in cubes)
        {
            cubicles.Enqueue(c);
        }

        if (cubes.Length > 0)
            world.ModifyState("Free Cubicle", cubes.Length);
    }

    public  void AddPatient(GameObject p)
    {
        patientsQueue.Enqueue(p);
    }

    public GameObject UseCubicle()
    {
        if (cubicles.Count == 0)
            return null;
        return cubicles.Dequeue();
    }

    public void AddCubicle(GameObject c)
    {
        cubicles.Enqueue(c);
    }

    public  GameObject RemovePatient()
    {
        if (patientsQueue.Count == 0)
            return null;
        return patientsQueue.Dequeue();
    }


    private GWorld() { }

    public static GWorld Instance
    {
        get
        {
            return instance;
        }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
