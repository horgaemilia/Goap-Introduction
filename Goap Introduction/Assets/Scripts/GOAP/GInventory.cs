using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GInventory 
{
    List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject i)
    {
        items.Add(i);
    }

    public GameObject FindItemWithTag(string tag)
    {
        var item = items.Find(a => a.tag == tag);
        return item;
    }

    public void RemoveItem(GameObject i)
    {
        items.Remove(i);
    }
}
