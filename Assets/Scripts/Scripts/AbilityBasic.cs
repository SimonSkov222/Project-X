using System.Collections;
using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Hvad en evne skal indeholde og alle evener
//  skal arve fra denne classe
//
//////////////////////////////////////////////////////
public abstract class AbilityBasic : ScriptableObject, IButton
{

    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    #region
    protected float timeColddown = 0;
    protected PlayerController playerController;

    [SerializeField]
    [Range(1, 25)]
    protected int stack = 1;

    [SerializeField]
    [Range(1, 25)]
    protected int stackMax = 1;

    [SerializeField]
    [Range(0, 120)]
    protected int colddown = 0;

    [SerializeField]
    protected bool useButton = true;
    #endregion

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    #region
    public abstract string Name { get; }
    public bool UseButton { get { return useButton; } }
    public bool IsReloadingStack { get; set; }
    #endregion

    ///////////////////////////////
    //      Public Metods
    ///////////////////////////////
    #region

    /// <summary>
    /// Metoden der vil blive kaldt når evnen skal aktiveres
    /// </summary>
    public abstract void OnActivate();

    /// <summary>
    /// Bliver kaldt når objecet loades
    /// kan overskrives
    /// </summary>
    public virtual void OnLoaded(GameObject characterGo)
    {
        
        playerController = characterGo.GetComponent<PlayerController>();
        StartReloadStack();
    }

    /// <summary>
    /// bliver kaldt når man trykker på evne knappen
    /// og kalder en anden metode der kan overskrives
    /// </summary>
    public void OnButton_Click(object parameter)
    {
        OnButton_Activate(ButtonCall.Click);
    }

    /// <summary>
    /// bliver kaldt når man trykker på evne knappen
    /// og kalder en anden metode der kan overskrives
    /// </summary>
    public void OnButton_Down(object parameters)
    {
        OnButton_Activate(ButtonCall.Down);
    }

    /// <summary>
    /// bliver kaldt når man trykker på evne knappen
    /// og kalder en anden metode der kan overskrives
    /// </summary>
    public void OnButton_Hold(object parameters)
    {
        OnButton_Activate(ButtonCall.Hold);
    }

    /// <summary>
    /// bliver kaldt når man trykker på evne knappen
    /// og kalder en anden metode der kan overskrives
    /// </summary>
    public void OnButton_Up(object parameters)
    {
        OnButton_Activate(ButtonCall.Up);
    }
    #endregion

    ///////////////////////////////
    //      Protected Metods
    ///////////////////////////////
    #region

    /// <summary>
    /// bliver kaldt når man trykker på evne knappen.
    /// Default er at når man trykke knappen ned bliver
    /// OnActivate() kaldt og starter med reloade evnen igen
    /// 
    /// kan overskrives
    /// </summary>
    protected virtual void OnButton_Activate(ButtonCall arg1)
    {

        if (arg1 == ButtonCall.Down && stack > 0)
        {
            OnActivate();
            stack--;
            StartReloadStack();
        }
    }


    /// <summary>
    /// Reloader evnen
    /// kan overskrives
    /// </summary>
    protected virtual IEnumerator ReloadStack()
    {
        for (int i = 0; i < colddown; i++)
        {
            yield return new WaitForSeconds(1);
        }

        stack++;

        IsReloadingStack = false;
        StartReloadStack();
    }

    /// <summary>
    /// Start reload af evne
    /// </summary>
    protected void StartReloadStack()
    {
        if (stack < stackMax && !IsReloadingStack)
        {
            IsReloadingStack = true;
            
            playerController.StartCoroutine(ReloadStack());
        }

    }
    #endregion

}
