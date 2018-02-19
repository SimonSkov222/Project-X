using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour {
    NavMeshAgent agent;

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    public GameObject target;
    public MovementPattern m_Pattern = MovementPattern.Idle;
    [Range(1,15)]
    public float m_SpeedWalk = 2.5f;
    [Range(1, 15)]
    public float m_SpeedRun = 8f;
    [Range(0, 50)]
    public float m_FollowStopDisanceMin = 0;
    [Range(0, 50)]
    public float m_FollowStopDisanceMax = 0;
    [Range(0, 50)]
    public float m_DetectRadius = 35f;


    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private Vector3 centerPoint;
    private Vector3 lastPosition;
    private MovementPattern lastPattern;
    private bool lockOnTarget = false;
    private bool notifyHasSetDesination = true;


    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////
    void Start () {
        OnMovementPatternChange(m_Pattern, m_Pattern);
        lastPattern = m_Pattern;
        lastPosition = transform.position;

        agent = GetComponent<NavMeshAgent>();
        
        centerPoint = transform.position;

        


    }
	
	// Update is called once per frame
	void Update () {

        if (lastPattern != m_Pattern)
        {
            OnMovementPatternChange(m_Pattern, lastPattern);
            lastPattern = m_Pattern;
        }
        if (notifyHasSetDesination && Vector3.Distance(transform.position, agent.destination) <= 1.1f || agent.remainingDistance == 0f)
        {
            notifyHasSetDesination = false;
            OnAgentReachDestination(agent, m_Pattern);
        }

        OnAgentMoveingUpdate(agent, m_Pattern);
    }

    private Vector3 GetRandomSpot(Vector3 point)
    {
        Debug.Log("---- " + System.Convert.ToString(1 << 4, 2).PadLeft(8, '0'));
        Debug.Log("----- " + HelperBitwise.Plus(3, 4));
        Debug.Log("------ " + ((1 << 3) | (1 << 4)) );
        int mask = HelperBitwise.Plus(3,4);//(1 << 3) | (1 << 4);// 1 << 8;// (NavMesh.GetAreaFromName("WalkPath")+ NavMesh.GetAreaFromName("CarRoad"));
        float dis = 3000f;
        NavMeshHit hit;
        NavMeshHit[] hitCheck = new NavMeshHit[4];
        Vector3 closestPoint = new Vector3();
        Vector3 left = Vector3.left * 3;
        Vector3 forward = Vector3.forward * 3;

        Vector3 leftRadius = Vector3.left * 1f;
        Vector3 forwardRadius = Vector3.forward * 1f;

        bool? addLeft = null;
        bool? addForward = null;

        if (NavMesh.SamplePosition(point, out hit, dis, mask))
        {
            closestPoint = hit.position;
        }
        NavMesh.SamplePosition(closestPoint + leftRadius, out hitCheck[0], dis, mask);
        NavMesh.SamplePosition(closestPoint - leftRadius, out hitCheck[1], dis, mask);
        NavMesh.SamplePosition(closestPoint + forwardRadius, out hitCheck[2], dis, mask);
        NavMesh.SamplePosition(closestPoint - forwardRadius, out hitCheck[3], dis, mask);

        if (hitCheck[0].position != closestPoint + leftRadius)
        {
            addLeft = true;
        }
        if (hitCheck[1].position != closestPoint - leftRadius)
        {
            addLeft = false;
        }


        if (hitCheck[2].position != closestPoint + forwardRadius)
        {
            addForward = true;
        }
        if (hitCheck[3].position != closestPoint - forwardRadius)
        {
            addForward = false;
        }
        
        if (!addLeft.HasValue && NavMesh.SamplePosition(closestPoint + left, out hit, dis, mask))
        {
            if (closestPoint == hit.position)
            {
                addLeft = true;
            }
            else if (hit.position != closestPoint + left)
            {
                addLeft = false;
            }
        }

        if (!addForward.HasValue && NavMesh.SamplePosition(closestPoint + forward, out hit, dis, mask))
        {
            if (closestPoint == hit.position)
            {
                addForward = true;
            }
            else if (hit.position != closestPoint + forward)
            {
                addForward = false;
            }
        }

        if (addLeft.HasValue)
        {
            closestPoint += addLeft.Value ? Vector3.left * -1f : Vector3.left * 1f;
        }
        if (addForward.HasValue)
        {
            closestPoint += addForward.Value ? Vector3.forward * -1f : Vector3.forward * 1f;
        }
        Debug.Log("RE  " + closestPoint + " --- " + point);
        return closestPoint;
    }
    private Vector3 GetAnStandStillPosition(Vector3 position, params int[] navmaskLayerID)
    {
        int mask = HelperBitwise.Plus(navmaskLayerID);
        float dis = 3000f;
        NavMeshHit hit;
        NavMeshHit[] hitCheck = new NavMeshHit[4];
        Vector3 closestPoint = new Vector3();
        Vector3 left = Vector3.left * 3;
        Vector3 forward = Vector3.forward * 3;

        Vector3 leftRadius = Vector3.left * 1f;
        Vector3 forwardRadius = Vector3.forward * 1f;

        bool? addLeft = null;
        bool? addForward = null;

        if (NavMesh.SamplePosition(position, out hit, dis, mask))
        {
            closestPoint = hit.position;
        }
        NavMesh.SamplePosition(closestPoint + leftRadius, out hitCheck[0], dis, mask);
        NavMesh.SamplePosition(closestPoint - leftRadius, out hitCheck[1], dis, mask);
        NavMesh.SamplePosition(closestPoint + forwardRadius, out hitCheck[2], dis, mask);
        NavMesh.SamplePosition(closestPoint - forwardRadius, out hitCheck[3], dis, mask);

        if (hitCheck[0].position != closestPoint + leftRadius)
        {
            addLeft = true;
        }
        if (hitCheck[1].position != closestPoint - leftRadius)
        {
            addLeft = false;
        }


        if (hitCheck[2].position != closestPoint + forwardRadius)
        {
            addForward = true;
        }
        if (hitCheck[3].position != closestPoint - forwardRadius)
        {
            addForward = false;
        }

        if (!addLeft.HasValue && NavMesh.SamplePosition(closestPoint + left, out hit, dis, mask))
        {
            if (closestPoint == hit.position)
            {
                addLeft = true;
            }
            else if (hit.position != closestPoint + left)
            {
                addLeft = false;
            }
        }

        if (!addForward.HasValue && NavMesh.SamplePosition(closestPoint + forward, out hit, dis, mask))
        {
            if (closestPoint == hit.position)
            {
                addForward = true;
            }
            else if (hit.position != closestPoint + forward)
            {
                addForward = false;
            }
        }

        if (addLeft.HasValue)
        {
            closestPoint += addLeft.Value ? Vector3.left * -1f : Vector3.left * 1f;
        }
        if (addForward.HasValue)
        {
            closestPoint += addForward.Value ? Vector3.forward * -1f : Vector3.forward * 1f;
        }

        return closestPoint;
    }

    private Vector3 GetRandomPointInDonut(Vector3 center, float minRadius, float maxRadius)
    {
        float rot = Random.Range(1f, 360f);
        Vector3 direction = Quaternion.AngleAxis(rot, Vector3.up) * Vector3.forward;
        Ray ray = new Ray(Vector3.zero, direction);

        return center + ray.GetPoint(Random.Range(minRadius, maxRadius));
    }
    
    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    
    private void OnMovementPatternChange(MovementPattern newPattern, MovementPattern oldPattern)
    {
        switch (newPattern)
        {
            case MovementPattern.Idle:
                break;
            case MovementPattern.FollowTarget:
            case MovementPattern.EnemyWalkBackAndForth:
            case MovementPattern.RunAwayFromTarget:
                agent.SetAreaCost(NavMesh.GetAreaFromName("Walkable"), 1);
                agent.SetAreaCost(NavMesh.GetAreaFromName("CarRoad"), 1);
                agent.SetAreaCost(NavMesh.GetAreaFromName("WalkPath"), 1);
                break;
            case MovementPattern.FriendlyWalkBackAndForth:
                agent.SetAreaCost(NavMesh.GetAreaFromName("Walkable"), 5);
                agent.SetAreaCost(NavMesh.GetAreaFromName("CarRoad"), 10);
                agent.SetAreaCost(NavMesh.GetAreaFromName("WalkPath"), 1);
                break;
        }

        switch (newPattern)
        {
            case MovementPattern.FriendlyWalkBackAndForth:
            case MovementPattern.EnemyWalkBackAndForth:
                agent.speed = m_SpeedWalk;
                break;
            case MovementPattern.FollowTarget:
            case MovementPattern.RunAwayFromTarget:
                agent.speed = m_SpeedRun;
                break;
        }
    }

    private void OnAgentReachDestination(NavMeshAgent agent, MovementPattern pattern)
    {
        switch (pattern)
        {
            case MovementPattern.Idle:
                break;
            case MovementPattern.FollowTarget:
                break;
            case MovementPattern.FriendlyWalkBackAndForth:
                StartCoroutine(WalkInCircles(agent, centerPoint, NavMesh.GetAreaFromName("WalkPath")));
                break;
            case MovementPattern.EnemyWalkBackAndForth:
                StartCoroutine(WalkInCircles(agent, centerPoint, NavMesh.GetAreaFromName("Walkable"), NavMesh.GetAreaFromName("CarRoad"), NavMesh.GetAreaFromName("WalkPath")));
                break;
            case MovementPattern.RunAwayFromTarget:
                RunAwayFromTarget(agent, target.transform.position);
                break;
        }

        //agent.SetDestination(GetRandomSpot(GetRandomPointInDonut(centerPoint, 2f, 5f)));

    }

    private void OnAgentMoveingUpdate(NavMeshAgent agent, MovementPattern pattern)
    {
        switch (pattern)
        {
            case MovementPattern.FollowTarget:
                FollowTarget(agent, target.transform.position);
                break;
        }
        
    }
    
    private void RunAwayFromTarget(NavMeshAgent agent, Vector3 targetPosition)
    {
        Vector3 agentPositon = agent.gameObject.transform.position;
        bool run = false;

        if (CanSeePlayer(targetPosition, 170f, m_DetectRadius) 
        || (lockOnTarget && CanSeePlayer(targetPosition, 360f, m_DetectRadius)))
        {
            lockOnTarget = true;
            run = true;
        }
        else
        {
            lockOnTarget = false;
        }

        if (run)
        {
            Vector3 direction = ((agentPositon) - (targetPosition)).normalized;
            Vector3 destination = agentPositon + direction * 12f;
            SetDestinationWithDistance(agent, destination);
        }
        else if(centerPoint != agent.destination)
        {
            Vector3 direction = ((agentPositon) - (targetPosition)).normalized;
            Vector3 destination = GetAnStandStillPosition(agentPositon, 4);//

            if (Vector3.Distance(targetPosition, destination) >= m_DetectRadius)
            {
                SetDestinationWithDistance(agent, destination);
                centerPoint = destination;
            }
        }

        
    }
    
    private void FollowTarget(NavMeshAgent agent, Vector3 targetPosition)
    {
        Vector3 agentPositon = agent.gameObject.transform.position;
        

        if (CanSeePlayer(targetPosition, 170f, m_DetectRadius))
        {
            lockOnTarget = true;
            float dis = Vector3.Distance(agentPositon, targetPosition);
            if (dis < m_FollowStopDisanceMin || dis >= m_FollowStopDisanceMax)
            {
                SetDestinationWithDistance(agent, targetPosition, m_FollowStopDisanceMin);
                RotateTowards(targetPosition, 10f);
            }
            
        }
        else if (lockOnTarget && CanSeePlayer(targetPosition, 360f, m_DetectRadius))
        {
            SetDestinationWithDistance(agent, targetPosition, m_FollowStopDisanceMin);
        }
        else
        {
            SetDestinationWithDistance(agent, agentPositon);
            lockOnTarget = false;
        }


    }

    private IEnumerator WalkInCircles(NavMeshAgent agent, Vector3 centerPoint, params int[] areaID)
    {
        yield return new WaitForSeconds(Random.Range(15, 30));
        Vector3 pointInDonut = GetRandomPointInDonut(centerPoint, 5f, 10f);
        Vector3 destination = GetAnStandStillPosition(pointInDonut, areaID);
        SetDestinationWithDistance(agent, destination);
    }
    private void RotateTowards(Vector3 target, float rotationSpeed)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    

    private void SetDestinationWithDistance(NavMeshAgent agent, Vector3 destination, float distance = 0f)
    {
        Vector3 agentPositon = agent.gameObject.transform.position;
        Vector3 direction = ((agentPositon) - (destination)).normalized;
        agent.SetDestination(destination + direction * distance);

        notifyHasSetDesination = true;
    }


    /// <summary>
    /// Sørger for at AI's trigger kun virker foran den
    /// </summary>
    /// <param name="canBeSeen">Det object som skal blive set</param>
    /// <returns>Returner true hvis noget er foran Ai</returns>
    private bool CanSeePlayer(Vector3 targetPositon, float viewDegrees, float radius)
    {

        RaycastHit hit;
        Vector3 rayDirection = targetPositon - transform.position;
        // Tjekker på om der er noget inden for den angle som vi laver
        if ((Vector3.Angle(rayDirection, transform.forward)) <= viewDegrees * 0.5f)
            if (Physics.Raycast(transform.position, rayDirection, out hit, radius))
                return (hit.transform.CompareTag("Player"));

        return false;
    }
}


public enum MovementPattern
{
    Idle,
    FollowTarget,
    FriendlyWalkBackAndForth,
    EnemyWalkBackAndForth,
    RunAwayFromTarget,
}

public static class HelperBitwise
{
    public static int Plus(params int[] value)
    {
        int result = 0;

        foreach (var val in value)
        {
            result |= 1 << val;
        }

        return result;
    }
}