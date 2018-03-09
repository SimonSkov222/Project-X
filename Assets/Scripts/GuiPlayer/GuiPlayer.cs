using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GuiPlayer : MonoBehaviour {

    public float minmapHeight = 10;
    public Camera minmap;
    public TextMeshProUGUI textAmmo;
    public TextMeshProUGUI textHealth;

    public Image ultimate;
    public Image ability1;
    public Image ability2;
    public Image ability3;

    public Image buff1;
    public Image buff2;
    public Image buff3;
    
    public static GuiPlayer Singleton;

    private GameObject player;
    
    public bool ComponentsLoaded { get; set; }
    public GameObject Player { get { return player; } set { player = value; ComponentsLoaded = false; } }

    private Canvas canvas;
    private PlayerController pc;

    void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if (Player == null)
        {
            canvas.enabled = false;
            return;
        }

        if (!ComponentsLoaded)
        {
            GetComponents();
        }

        canvas.enabled = true;
        textAmmo.text = pc.Weapon.Ammo.ToString();
        textHealth.text = pc.Health.ToString();
        minmap.transform.position = player.transform.position + (Vector3.up * minmapHeight);


    }

    private void GetComponents()
    {
        
        pc = Player.GetComponent<PlayerController>();
        ComponentsLoaded = true;
    }
}
