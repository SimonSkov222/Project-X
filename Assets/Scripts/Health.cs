using UnityEngine;
using UnityEngine.UI;


////////////////////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Giver et gameobject liv og mulighed for at kunne tage skade
//
////////////////////////////////////////////////////////////////////
public class Health : MonoBehaviour
{
    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public bool IsAlive { get { return currentHP > 0; } }
    public bool UseHealthBar { get { return healthBarTemp != null; } }


    ///////////////////////////////
    //      Public Fieds
    ///////////////////////////////
    public Canvas healthBarTemp;
    public Vector3 healthBarPosition;
    public GameObject[] hitboxs;
    public int currentHP;
    public int maxHP = 500;

    ///////////////////////////////
    //      Private Fieds
    ///////////////////////////////
    private Canvas healthBar;
    private Slider healthBarSlider;


    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////

    /// <summary>
    /// Giver gameobjectet fuld liv
    /// og tildeler den metode der skal udføres når en
    /// hitbox bliver ramt
    /// 
    /// Hvis der er tilføjet et template for healthbar
    ///  vil den også blive oprettet her
    /// </summary>
    void Start()
    {
        currentHP = maxHP;
        
        for (int i = 0; i < hitboxs.Length; i++)
            hitboxs[i].GetComponent<Hitbox>().OnTakeDamge  = TakeDamage;

        if (UseHealthBar)
        {
            healthBar = Instantiate<Canvas>(healthBarTemp);
            healthBar.transform.SetParent(transform);
            healthBar.transform.localPosition = healthBarPosition;
            healthBarSlider = healthBar.GetComponentInChildren<Slider>();
            healthBarSlider.maxValue = maxHP;
            healthBarSlider.value = currentHP;
        }
        
    }
    
    /// <summary>
    /// Når gameobjectet dør vil vi gør den usynlig
    /// </summary>
    void Update() { if (!IsAlive && gameObject.activeSelf)  gameObject.SetActive(false);  }


    ///////////////////////////////
    //      Public Method
    ///////////////////////////////

    /// <summary>
    /// Giver damage til dette gameobject 
    /// Denne metode bliver kaldt fra hitboxne
    /// 
    /// Hvis der bruges health bar vil den
    ///  også blive opdateret her
    /// </summary>
    /// <param name="dmg">Træk fra i liv(currentHP)</param>
    public void TakeDamage(int dmg)
    {
        currentHP -= dmg < currentHP ? dmg : currentHP;

        if (UseHealthBar)
        {
            healthBarSlider.value = currentHP;
        }
    }
}
