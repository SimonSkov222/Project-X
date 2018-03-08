using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Spawner et skjold som man kan gemme sig bag ved
//
//////////////////////////////////////////////////////
[CreateAssetMenu(fileName = "RaiShield", menuName = "Abilities/Rai/Shield")]
public class RaiShield : AbilityBasic
{

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region
    [SerializeField]
    private GameObject shieldModel;

    [SerializeField]
    private int health;
    [SerializeField]
    private int upTime;


    private List<GameObject> shieldPool = new List<GameObject>();
    private GameObject player;
    #endregion

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    #region
    public override string Name { get { return "Rai Shield"; } }
    #endregion

    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region
    /// <summary>
    /// Gør klar til at bruge evenen.
    /// Laver array af shields
    /// </summary>
    public override void OnLoaded(GameObject characterGo)
    {
        base.OnLoaded(characterGo);
        player = characterGo;
        for (int i = 0; i < stackMax; i++)
        {
            GameObject shield = Instantiate<GameObject>(shieldModel);
            NetworkIdentity ni = shield.AddComponent<NetworkIdentity>();
            SimpleHealth health = shield.AddComponent<SimpleHealth>();

            ni.localPlayerAuthority = true;

            health.HealthMax = this.health;
            health.EventOnEnable += (s) => HealthHelper.Initialize(ni.netId.Value, s);
            health.EventOnDeath += (s,o) => s.SetActive(false);
            shield.SetActive(false);
            shieldPool.Add(shield);
        }
    }

    /// <summary>
    /// Spawner et nyt shield
    /// </summary>
    public override void OnActivate()
    {
        var shield = GetInactiveShield() ?? GetFirstSpawnedShield();
        if (shield != null)
        {
            shield.transform.position = player.transform.position;
            shield.transform.position += player.transform.forward;
            shield.transform.rotation = player.transform.rotation;
            shield.SetActive(true);
        }

    }

    #endregion

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Henter et deaktiveret shield
    /// </summary>
    private GameObject GetInactiveShield()
    {
        var shield = shieldPool.FirstOrDefault(m => m != null && !m.activeSelf);
        return shield;
    }

    /// <summary>
    /// Henter det første shield.
    /// TODO Gør at den tager efter tid.
    /// </summary>
    private GameObject GetFirstSpawnedShield()
    {
        if (shieldPool.Count > 0)
        {
            return shieldPool[0];
        }
        return null;
        
    }
    #endregion







}
