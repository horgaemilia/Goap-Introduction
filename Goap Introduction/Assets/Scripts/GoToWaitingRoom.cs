using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingRoom : GAction
{
    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("patientWaiting", 1);
        beliefs.ModifyState("atHospital", 1);
        GWorld.Instance.AddPatient(gameObject);
        return true;
    }

    public override bool PrePerform()
    {
        return true;
    }
}
