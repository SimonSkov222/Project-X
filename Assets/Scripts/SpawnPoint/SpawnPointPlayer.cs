using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPlayer : MonoBehaviour {

    public GameObject player;
    public GameObject otherPlayers;

    private bool reSpawn;
    private bool isDead;


    int i = 2;

    // Use this for initialization
    void Start ()
    {
        otherPlayers.transform.position = transform.position + transform.forward * 2;


    }
	
	// Update is called once per frame
	void Update () {


        Spawn(player);

        if (isDead == true && reSpawn == true)
        {
            Spawn(player);
        }

	}
    

    private void Spawn(GameObject deadPlayer)
    {
        var playerPos = deadPlayer.transform.position + transform.forward * i;
        Debug.Log("1 " + i);
        if (otherPlayers.transform.position == playerPos)
        {
            i++;
        }
        Debug.Log("2 " + i);
        deadPlayer.transform.position = transform.position + transform.forward * i;
        
        

        //deadPlayer.transform.position = transform.position + new Vector3(Random.Range(3f, 6f),0, Random.Range(3f, 6f));
        
    }
    

}
