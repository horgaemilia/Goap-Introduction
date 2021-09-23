using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treat : GAction
{
    public override bool PostPerform()
    {
        return true;
    }

    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
            return false;
        return true;
    }
}
