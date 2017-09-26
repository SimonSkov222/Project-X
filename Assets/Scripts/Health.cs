using UnityEngine;

public class Health : MonoBehaviour
{



    public int currentHP;
    public int HP = 500;
    public GameObject[] Hitboxs;
    public bool IsAlive { get { return currentHP > 0; } }
    

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
        if (!IsAlive)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP += dmg < currentHP ? -dmg : -currentHP;
        //Debug.Log("dmg " + dmg);
        //Debug.Log(gameObject.name);
        //Debug.Log("cpcp" + currentHP);

    }
    


}
