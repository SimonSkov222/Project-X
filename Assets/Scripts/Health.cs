using UnityEngine;


//////////////////////////////////////////////////////
//      Public Properties
//  
//  asdasdasdsdfasfasfd
//  sdfsdfsdfgfgfgfgfgfgfgfgfgfgfgfgfgfgfgfgfh
//  
//////////////////////////////////////////////////////
public class Health : MonoBehaviour
{


    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public bool IsAlive { get { return currentHP > 0; } }


    ///////////////////////////////
    //      Public Fieds
    ///////////////////////////////
    public int currentHP;
    public int HP = 500;
    public GameObject[] Hitboxs;
    

    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////
    void Start()
    {
        currentHP = HP;
        
        for (int i = 0; i < Hitboxs.Length; i++)
        {
            Hitboxs[i].GetComponent<Hitbox>().OnTakeDamge  = TakeDamage;
        }
    }
    
    void Update()
    {
        /*
         *      
         */
        if (!IsAlive)
        {
            gameObject.SetActive(false);
        }

        //  sdfsdfsdfsd
        if (!IsAlive)
        {
            gameObject.SetActive(false);
        }
    }


    ///////////////////////////////
    //      Public Method
    ///////////////////////////////

    /// <summary>
    /// Giver damage til dette gameobject
    /// </summary>
    /// <param name="dmg">Træk fra i liv</param>
    public void TakeDamage(int dmg)
    {
        currentHP += dmg < currentHP ? -dmg : -currentHP;

    }
    


}
