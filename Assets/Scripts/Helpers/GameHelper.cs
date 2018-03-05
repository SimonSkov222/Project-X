using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper {


    /// <summary>
    /// Sørger for at AI's trigger kun virker foran den
    /// </summary>
    /// <param name="canBeSeen">Det object som skal blive set</param>
    /// <returns>Returner true hvis noget er foran Ai</returns>
    public static bool CanSeePlayer(GameObject sender, GameObject target, float viewDegrees, float radius)
    {
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - sender.transform.position;
        // Tjekker på om der er noget inden for den angle som vi laver
        if ((Vector3.Angle(rayDirection, sender.transform.forward)) <= viewDegrees * 0.5f)
            if (Physics.Raycast(sender.transform.position, rayDirection, out hit, radius))
                return (hit.collider.gameObject.GetInstanceID() == target.GetInstanceID());

        return false;
    }
}
