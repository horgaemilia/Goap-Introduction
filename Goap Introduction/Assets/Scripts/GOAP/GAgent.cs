using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> goals;

    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        goals = new Dictionary<string, int>();
        goals.Add(s, i);
        remove = r;
    }
}
public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    // Start is called before the first frame update
    protected void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach(GAction a in acts)
        {
            actions.Add(a);
        }
    }

    bool invoked = false;

    private void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(currentAction != null && currentAction.running)
        {
            if(currentAction.agent.hasPath && currentAction.agent.remainingDistance < 1f)
            {
                if(!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }
        if(planner == null || actionQueue == null)
        {
            planner = new GPlanner();
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach(KeyValuePair<SubGoal, int> sub in sortedGoals)
            {
                actionQueue = planner.Plan(actions, sub.Key.goals, null);
                if(actionQueue!= null)
                {
                    currentGoal = sub.Key;
                    break;
                }
            }
        }
        if(actionQueue != null && actionQueue.Count == 0)
        {
            if(currentGoal.remove)
            {
                goals.Remove(currentGoal);
                planner = null;
            }
        }

        if(actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if(currentAction.PrePerform())
            {
                if(currentAction.target == null && currentAction.tag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }
                if(currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                actionQueue = null;
            }

        }
    }
}
