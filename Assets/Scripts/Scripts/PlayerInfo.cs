using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public TextMeshProUGUI Name;
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI Level;

    int LevelI = 2;

    public Image PictureIcon;
    public Image PictureCharacter;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Name.text = "!!!!!!!!Name!!!!!!!";
        CharacterName.text = "!!!!!!!!CharacterName!!!!!!!!!!!";
        Level.text = LevelI.ToString();
        //PictureIcon = PictureIcon
        //PictureCharacter = PictureCharacter


	}
}
