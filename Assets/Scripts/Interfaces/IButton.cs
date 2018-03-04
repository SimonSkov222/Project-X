using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButton
{
    void OnButton_Hold(object parameter);
    void OnButton_Down(object parameter);
    void OnButton_Up(object parameter);
    void OnButton_Click(object parameter);
}
