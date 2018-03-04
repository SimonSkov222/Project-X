using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RaiShield", menuName = "Abilities/Rai/Shield")]
public class RaiShield : AbilityBasic
{
    [SerializeField]
    private GameObject shieldModel;

    [SerializeField]
    private int health;
    [SerializeField]
    private int upTime;


    private List<GameObject> shieldPool = new List<GameObject>();
    private GameObject player;

    public override string Name { get { return "Rai Shield"; } }

    public override void OnLoaded(GameObject characterGo)
    {
        base.OnLoaded(characterGo);

        player = characterGo;
        for (int i = 0; i < stackMax; i++)
        {
            GameObject shield = Instantiate<GameObject>(shieldModel);
            shield.SetActive(false);
            shieldPool.Add(shield);
        }
    }
    

    public override void OnActivate()
    {
        Debug.Log("Spawn Shield");

        var shield = GetInactiveShield() ?? GetFirstSpawnedShield();
        if (shield != null)
        {
            shield.transform.position = player.transform.position;
            shield.transform.position += player.transform.forward;
            shield.transform.rotation = player.transform.rotation;
            shield.SetActive(true);
        }

    }



    private GameObject GetInactiveShield()
    {
        var shield = shieldPool.FirstOrDefault(m => m != null && !m.activeSelf);
        return shield;
    }

    private GameObject GetFirstSpawnedShield()
    {
        return shieldPool[0];
    }
}
