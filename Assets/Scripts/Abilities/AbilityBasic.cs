using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBasic : ScriptableObject, IButton
{
    public abstract string Name { get; }
    private float timeColddown = 0;

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

    public bool UseButton { get { return useButton; } }
    public bool IsReloadingStack = false;
    
    public void OnButton_Click(object parameter)
    {
        OnButton_Activate(ButtonCall.Click);
    }

    public void OnButton_Down(object parameters)
    {
        OnButton_Activate(ButtonCall.Down);
    }

    public void OnButton_Hold(object parameters)
    {
        OnButton_Activate(ButtonCall.Hold);
    }

    public void OnButton_Up(object parameters)
    {
        OnButton_Activate(ButtonCall.Up);
    }

    protected virtual void OnButton_Activate(ButtonCall arg1)
    {
        
        if (arg1 == ButtonCall.Down && stack > 0)
        {
            OnActivate();
            stack--;
            StartReloadStack();
        }
    }

    public virtual void OnLoaded(GameObject characterGo)
    {
        StartReloadStack();
    }

    public abstract void OnActivate();

    protected virtual IEnumerator ReloadStack()
    {
        for (int i = 0; i < colddown; i++)
        {
            yield return new WaitForSeconds(1);
        }

        stack++;
        Debug.Log("New shild Addet");
        IsReloadingStack = false;
        StartReloadStack();
    }

    protected void StartReloadStack()
    {
        if (stack < stackMax && !IsReloadingStack)
        {
            IsReloadingStack = true;
            PlayerController.Instance.StartCoroutine(ReloadStack());
        }

    }
    
}
