using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();

    private static Queue<GameObject> patientsQueue;

    private static WorldStates world;

    static GWorld()
    {
        world = new WorldStates();
        patientsQueue = new Queue<GameObject>();
    }

    public  void AddPatient(GameObject p)
    {
        patientsQueue.Enqueue(p);
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
