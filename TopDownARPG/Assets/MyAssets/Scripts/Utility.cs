using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Utility
{
    public static bool CalculateNewPath(NavMeshAgent _agent, Vector3 _destination)
    {
        NavMeshPath navMeshPath = new NavMeshPath();

        _agent.CalculatePath(_destination, navMeshPath);


        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("New path calculated (false)");
            return false;
        }
        else
        {
            Debug.Log("New path calculated (true)");
            return true;
        }
    }


}
