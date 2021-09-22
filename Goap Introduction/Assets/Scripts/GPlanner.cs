using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node parentNode;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public Node(Node parent,float cost, Dictionary<string,int> allstates, GAction action)
    {
        this.parentNode = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }
}
public class GPlanner 
{

    public Queue<GAction> Plan(List<GAction> actions,Dictionary<string,int> goal, WorldState state)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (a.IsAchievable())
                usableActions.Add(a);
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if(!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(cheapest.cost > leaf.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<GAction> result = new List<GAction>();
        Node n = cheapest;
        while(n.action != null)
        {
            result.Insert(0,n.action);
            n = n.parentNode;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("The plan is: ");
        queue.ToList().ForEach(a => Debug.Log("Q" + a.actionName));
        return queue;
    }

    private bool BuildGraph(Node parent,List<Node> leaves,List<GAction> usableActions,Dictionary<string,int> goal)
    {
        bool foundPath = false;
        foreach(GAction action in usableActions)
        {
            if(action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string,int> effect in action.effects)
                {
                    if(!currentState.ContainsKey(effect.Key))
                    {
                        currentState.Add(effect.Key, effect.Value);
                    }
                }
                Node node = new Node(parent,parent.cost + action.cost,currentState,action);
                if(GoalAchieved(goal,currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if(found)
                    {
                        foundPath = true;
                    }
                }
      
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string,int> goal,Dictionary<string,int> state)
    {
        foreach(KeyValuePair<string,int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removedAction)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (!a.Equals(removedAction))
                subset.Add(a);
        }
        return subset;

    }

}
