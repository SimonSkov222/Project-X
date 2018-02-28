using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButton
{
    void OnButtonHold(GameObject player, object weapon);
    void OnButtonDown(GameObject player, object weapon);
    void OnButtonUp(GameObject player, object weapon);
    void OnButtonClick(GameObject player, object weapon);
}
