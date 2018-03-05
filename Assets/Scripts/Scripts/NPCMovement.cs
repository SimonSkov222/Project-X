using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Håndtere hvordan vores NCP skal bevæge sig.
//  Vi kan f.eks. sige at vores NPC skal løbe væk
//  fra et GameObject eller hent til 
//  et GameObject via. target variablen.
//  
//  vi kan også sige at vores NPC bara skal gå lidt
//  rundt.
//
//////////////////////////////////////////////////////
[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour {

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    #region

    public GameObject target;
    public MovementPattern m_Pattern = MovementPattern.Idle;
    [Range(1,15)]
    public float m_SpeedWalk = 2.5f;
    [Range(1, 15)]
    public float m_SpeedRun = 8f;
    [Range(0, 50)]
    public float m_FollowStopDistanceMin = 0;
    [Range(0, 50)]
    public float m_FollowStopDistanceMax = 0;
    [Range(0, 50)]
    public float m_DetectRadius = 35f;
    [Range(0, 50)]
    public int m_WalkBackAndForthWaitMin = 5;
    [Range(0, 50)]
    public int m_WalkBackAndForthWaitMax = 10;

    #endregion

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region

    private NavMeshAgent agent;
    private Vector3 centerPoint;
    private Vector3 targetLastPosition;
    private MovementPattern lastPattern;
    private bool lockOnTarget = false;
    private bool notifyHasSetDesination = true;
    private Coroutine cRunningPattern;
    private Coroutine cLookAtTarget;

    #endregion

    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////
    #region

    /// <summary>
    /// Vi henter de components scriptet skal bruge.
    /// Sætter start indstillinger.
    /// </summary>
    void Start ()
    {
        //Hent compontents
        agent = GetComponent<NavMeshAgent>();

        //Sæt indstillinger for dette mønster
        OnMovementPatternChange(m_Pattern, m_Pattern);
        lastPattern = m_Pattern;
    }

    /// <summary>
    /// Styre hvornår vi skal kalde On* metoder (f.eks. OnMovementPatternChange)
    /// </summary>
    void Update ()
    {
        
        // Event for når man skifter m_Pattern
        if (lastPattern != m_Pattern)
        {
            OnMovementPatternChange(m_Pattern, lastPattern);
            lastPattern = m_Pattern;
        }

        // Stop fra at udføre de pattern der skal have et target
        switch (m_Pattern)
        {
            case MovementPattern.FollowTargetWhenSeen:
            case MovementPattern.FollowTargetAlways:
            case MovementPattern.RunAwayFromTarget:
                if (target == null)
                {
                    return;
                }
                break;
        }

        // Event for når man har nået sin desination
        if (notifyHasSetDesination && (Vector3.Distance(transform.position, agent.destination) <= 1.1f || agent.remainingDistance == 0f))
        {
            notifyHasSetDesination = false;
            OnAgentReachDestination(agent, m_Pattern);
        }

        // Event for når target har flyttet sig
        if (target != null && target.transform.position != targetLastPosition)
        {
            targetLastPosition = target.transform.position;
            OnTargetHasMoved(agent, m_Pattern);
        }


        // Gør at man start langsom og stopper hurtigt
        if (agent.hasPath)
        {
            agent.acceleration = (agent.remainingDistance < agent.stoppingDistance) ? 60f : 8f;
        }
    }

    #endregion
    

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Bliver kladt når man ændre m_Pattern.
    /// Vi ændre area cost for vores agent og 
    /// sætter hvor hurtig vores agent skal være.
    /// </summary>
    private void OnMovementPatternChange(MovementPattern newPattern, MovementPattern oldPattern)
    {
        //Gør at OnAgentReachDestination kan blive kaldt.
        notifyHasSetDesination = true;

        // Set area cost.
        switch (newPattern)
        {
            case MovementPattern.EnemyWalkBackAndForth:
            case MovementPattern.RunAwayFromTarget:
                agent.SetAreaCost(NavMesh.GetAreaFromName("Walkable"), 1);
                agent.SetAreaCost(NavMesh.GetAreaFromName("CarRoad"), 1);
                agent.SetAreaCost(NavMesh.GetAreaFromName("WalkPath"), 1);

                centerPoint = transform.position;
                break;
            case MovementPattern.FriendlyWalkBackAndForth:
                agent.SetAreaCost(NavMesh.GetAreaFromName("Walkable"), 5);
                agent.SetAreaCost(NavMesh.GetAreaFromName("CarRoad"), 10);
                agent.SetAreaCost(NavMesh.GetAreaFromName("WalkPath"), 1);

                //skift centerpoint til at være et fortov
                centerPoint = GetAnStandStillPosition(transform.position, NavMesh.GetAreaFromName("WalkPath"));
                break;
        }

        // Agent speed.
        switch (newPattern)
        {
            case MovementPattern.FriendlyWalkBackAndForth:
            case MovementPattern.EnemyWalkBackAndForth:
                agent.speed = m_SpeedWalk;
                break;
            case MovementPattern.FollowTargetWhenSeen:
            case MovementPattern.FollowTargetAlways:
            case MovementPattern.RunAwayFromTarget:
                agent.speed = m_SpeedRun;
                break;
        }
        
    }

    /// <summary>
    /// Bliver kaldt når vores agent har nået sin destination.
    /// Udføre metoder for hvor vores agent skal gå hent.
    /// /// </summary>
    private void OnAgentReachDestination(NavMeshAgent agent, MovementPattern pattern)
    {
        //Stopper cRunningPattern
        CancelRunningCoroutine();

        switch (pattern)
        {
            case MovementPattern.FriendlyWalkBackAndForth:
                cRunningPattern = StartCoroutine(WalkInCircles(agent, centerPoint, NavMesh.GetAreaFromName("WalkPath")));
                break;
            case MovementPattern.EnemyWalkBackAndForth:
                cRunningPattern = StartCoroutine(WalkInCircles(agent, centerPoint, NavMesh.GetAreaFromName("Walkable"), NavMesh.GetAreaFromName("CarRoad"), NavMesh.GetAreaFromName("WalkPath")));
                break;
            case MovementPattern.RunAwayFromTarget:
                RunAwayFromTarget(agent, target);
                break;
        }
    }

    /// <summary>
    /// Bliver kladt når vores target ændre position.
    /// Udføre metoder for hvor vores agent skal gå hent.
    /// </summary>
    private void OnTargetHasMoved(NavMeshAgent agent, MovementPattern pattern)
    {
        switch (pattern)
        {
            case MovementPattern.FollowTargetAlways:
                FollowTarget(agent, target, false);
                break;
            case MovementPattern.FollowTargetWhenSeen:
                FollowTarget(agent, target, true);
                break;
        }
        
    }
    

    /// <summary>
    /// Gør at vores NPC vil løbe i modsat retning af target.
    /// Target skal være foran NPC før den begynder at løbe.
    /// </summary>
    private void RunAwayFromTarget(NavMeshAgent agent, GameObject target)
    {
        Vector3 agentPositon = agent.gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        bool run = false;

        //Tjekker om target er foran NPC eller target har været foran NPC og er nu ligebage npc
        if (CanSeePlayer(target, 170f, m_DetectRadius) 
        || (lockOnTarget && CanSeePlayer(target, 360f, m_DetectRadius)))
        {
            lockOnTarget = true;
            run = true;
        }
        else
        {
            lockOnTarget = false;
        }

        //Løb væk fra target
        if (run)
        {
            Vector3 direction = ((agentPositon) - (targetPosition)).normalized;
            Vector3 destination = agentPositon + direction * 12f;
            SetDestinationWithDistance(agent, destination);
        }
        //Hvis target ikke løbe efter NPC mere skal vores NPC løbe tilbage til fortovet.
        else if(centerPoint != agent.destination)
        {
            //Tjek om NPC ikke kommer for tæt på target
            Vector3 destination = GetAnStandStillPosition(agentPositon, NavMesh.GetAreaFromName("WalkPath"));//

            if (Vector3.Distance(targetPosition, destination) >= m_DetectRadius)
            {
                SetDestinationWithDistance(agent, destination);
                centerPoint = destination;
            }
        }
    }
    
    /// <summary>
    /// Gør at vores NPC vil følge efter Target.
    /// Man kan vælge om vores NPC skal se target først 
    /// eller om den bare skal løbe der hen.
    /// </summary>
    private void FollowTarget(NavMeshAgent agent, GameObject target, bool whenSeen)
    {
        Vector3 agentPositon = agent.gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        float dis = Vector3.Distance(agentPositon, targetPosition);

        //Løb altid hent til target
        if (!whenSeen)
        {
            if (!CanSeePlayer(target, 360f, m_DetectRadius) || dis < m_FollowStopDistanceMin || dis >= m_FollowStopDistanceMax)
            {
                CancelRunningCoroutine();
                SetDestinationWithDistance(agent, targetPosition, m_FollowStopDistanceMax);
                cLookAtTarget = StartCoroutine(LookAtTarget(target));
            }
        }
        // Løb hent til target når den har set den.
        else if (CanSeePlayer(target, 170f, m_DetectRadius))
        {
            CancelRunningCoroutine();
            cLookAtTarget = StartCoroutine(LookAtTarget(target));
            lockOnTarget = true;
            if (dis < m_FollowStopDistanceMin || dis >= m_FollowStopDistanceMax)
            {
                SetDestinationWithDistance(agent, targetPosition, m_FollowStopDistanceMax);
            }
            
        }
        //Forsæt med at løbe hent til target når target er blevet set.
        else if (lockOnTarget && CanSeePlayer(target, 360f, m_DetectRadius))
        {
            SetDestinationWithDistance(agent, targetPosition, m_FollowStopDistanceMax);

        }
        //Stop med at løbe.
        else
        {
            SetDestinationWithDistance(agent, agentPositon);
            lockOnTarget = false;
            CancelRunningCoroutine();
        }

    }

    /// <summary>
    /// Gør at vores NPC kigger på target
    /// hvis det er muligt at se target.
    /// </summary>
    private IEnumerator LookAtTarget(GameObject target)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (CanSeePlayer(target, 360f, m_DetectRadius))
            {
                RotateTowards(target.transform.position, 10f);
            }
        }
    }

    /// <summary>
    /// Gør at vores NPC gå hent til et tilfældige point indefor et radius.
    /// Poinet vil være indefor et area man selv har valgt.
    /// </summary>
    private IEnumerator WalkInCircles(NavMeshAgent agent, Vector3 centerPoint, params int[] areaID)
    {
        int sec = Random.Range(m_WalkBackAndForthWaitMin, m_WalkBackAndForthWaitMax);
        yield return new WaitForSeconds(sec);

        Vector3 pointInDonut = GetRandomPointInDonut(centerPoint, 2f, 4f);
        Vector3 destination = GetAnStandStillPosition(pointInDonut, areaID);
        SetDestinationWithDistance(agent, destination);
    }

    /// <summary>
    /// Skal blive kaldt hvis man vil give NPC en ny destination.
    /// Gør det muglit at NPC skal holde en afstand fra destination pointet.
    /// </summary>
    private void SetDestinationWithDistance(NavMeshAgent agent, Vector3 destination, float distance = 0f)
    {
        Vector3 agentPositon = agent.gameObject.transform.position;

        //Gør at vores NPC går væk fra destination.
        if (Vector3.Distance(agentPositon, destination) <= distance)
        {
            Vector3 direction = ((agentPositon) - (destination)).normalized;
            agent.SetDestination(destination + direction * distance);
            agent.stoppingDistance = 0;
        }
        //Gør at vores NPC går hen til destination med stopping distance.
        else
        {
            agent.SetDestination(destination);
            agent.stoppingDistance = distance;
        }
        

        notifyHasSetDesination = true;
    }

    /// <summary>
    /// Finder den tætteste position på den opgivet position som man må stå på.
    /// </summary>
    private Vector3 GetAnStandStillPosition(Vector3 position, params int[] navmaskLayerID)
    {
        int mask = 0;
        foreach (var val in navmaskLayerID)
        {
            mask |= 1 << val;
        }

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
        
        //Vector3[] checkPositions = {
        //    closestPoint + leftRadius,
        //    closestPoint - leftRadius,
        //    closestPoint + forwardRadius,
        //    closestPoint - forwardRadius
        //};

        //Finder den tætteste position
        if (NavMesh.SamplePosition(position, out hit, dis, mask))
        {
            closestPoint = hit.position;
        }

        //Dette skal tjekke om closestPoint er indefor det opgivet area
        //for (int i = 0; i < checkPositions.Length; i++)
        //{
        //    NavMeshHit hitCheck;
        //    NavMesh.SamplePosition(checkPositions[i], out hitCheck, dis, mask);

        //    if (hitCheck.position != checkPositions[i])
        //    {
        //        if (i > 2)
        //        {
        //            addLeft = i == 0;
        //        }
        //        else
        //        {
        //            addForward = i == 2;
        //        }
        //    }
        //}

        //Dette skal tjekke om closestPoint er indefor det opgivet area
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

        //Lige nu er closestPoint lige på kanten af area.
        //denne if skal tjekke hvilken side vil skal flytte
        //closestPoint hent til.
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

        //Samme som if`en foroven.
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

        //flytter closestPoint ind på area så det ikke står i kanten
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

    /// <summary>
    /// Beregner et tilfældigt position som er længere væk end minRadius og tættere på end maxRadius.
    /// </summary>
    private Vector3 GetRandomPointInDonut(Vector3 center, float minRadius, float maxRadius)
    {
        float rot = Random.Range(1f, 360f);
        Vector3 direction = Quaternion.AngleAxis(rot, Vector3.up) * Vector3.forward;
        Ray ray = new Ray(Vector3.zero, direction);

        return center + ray.GetPoint(Random.Range(minRadius, maxRadius));
    }

    /// <summary>
    /// Sørger for at AI's trigger kun virker foran den
    /// </summary>
    /// <param name="canBeSeen">Det object som skal blive set</param>
    /// <returns>Returner true hvis noget er foran Ai</returns>
    private bool CanSeePlayer(GameObject target, float viewDegrees, float radius)
    {

        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;
        // Tjekker på om der er noget inden for den angle som vi laver
        if ((Vector3.Angle(rayDirection, transform.forward)) <= viewDegrees * 0.5f)
            if (Physics.Raycast(transform.position, rayDirection, out hit, radius))
                return (hit.collider.gameObject.GetInstanceID() == target.GetInstanceID());

        return false;
    }

    /// <summary>
    /// I hvilken retning vores NPC skal kigge hen
    /// </summary>
    private void RotateTowards(Vector3 target, float rotationSpeed)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    /// <summary>
    /// Stopper nogle coroutine så der ikke er flere der køre samtidigt.
    /// </summary>
    private void CancelRunningCoroutine()
    {
        if (cRunningPattern != null)
        {
            StopCoroutine(cRunningPattern);
        }
        if (cLookAtTarget != null)
        {
            StopCoroutine(cLookAtTarget);
        }

        
    }
    
    #endregion
}
