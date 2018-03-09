using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpgraderingsStenItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Image itemImage;
    public Sprite stenImage;
    public OpgraderingsSten sten;
    private bool stenActive;
    public WeaponBasic weaponBasic;
    int i;

	// Use this for initialization
	void Start () {
        itemImage = GetComponent<Image>();
        i = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (sten != null && i == 0 && stenActive)
        {
            i++;
            Debug.Log("Sten er ikke null ");
            weaponBasic.addOpgraderingsSten(sten);

            if (weaponBasic.opgraderingsSten.Keys != null)
            {
                Debug.Log(weaponBasic.opgraderingsSten[0]);
            }
            Debug.Log("null");
        }
    }

    private Vector3 startPos;
    private Transform firstParent;

    public void OnBeginDrag(PointerEventData eventData)
    {

        startPos = transform.position;
        firstParent = transform.parent;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent != firstParent)
        {
            transform.position = startPos;
        }
    }



    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Debug.Log("mouse enter");
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("mouse down");
    //    transform.position = eventData.position;

    //}
}
