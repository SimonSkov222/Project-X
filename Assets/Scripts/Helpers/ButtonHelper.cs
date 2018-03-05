﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ButtonHelper {

    const float ClickTime = 0.4f;

    private static Dictionary<string, float> timeButtonDown = new Dictionary<string, float>();

    public static bool TryCallMethod(object sender, string method, params object[] parameters)
    {
        if (sender == null)
        {
            return false;
        }
        Type senderType = sender.GetType();
        MethodInfo methodInfo = senderType.GetMethod(method);
        if (methodInfo != null)
        {
            methodInfo.Invoke(sender, parameters);
            return true;
        }
        return false;
    }

    public static bool InvokeButtonEvents(object sender,string buttonName, string prefix, params object[] parameters)
    {
        if (sender == null)
        {
            return false;
        }

        bool hasMethod = false;
        if (!timeButtonDown.ContainsKey(buttonName))
        {
            timeButtonDown.Add(buttonName, 0);
        }

        Type senderType = sender.GetType();

        if (Input.GetButtonDown(buttonName))
        {
            timeButtonDown[buttonName] = Time.time;

            MethodInfo methodDown = senderType.GetMethod(prefix + "_Down");
            if (methodDown != null)
            {
                methodDown.Invoke(sender, parameters);
                hasMethod = true;
            }

        }
        if (Input.GetButton(buttonName))
        {
            MethodInfo methodHold = senderType.GetMethod(prefix + "_Hold");
            if (methodHold != null)
            {
                methodHold.Invoke(sender, parameters);
                hasMethod = true;
            }
        }
        if (Input.GetButtonUp(buttonName))
        {
            float pressTime = Time.time - timeButtonDown[buttonName];

            MethodInfo methodClick = senderType.GetMethod(prefix + "_Click");
            MethodInfo methodUp = senderType.GetMethod(prefix + "_Up");

            if (methodUp != null)
            {
                methodUp.Invoke(sender, parameters);
                hasMethod = true;
            }

            if (pressTime <= ClickTime && methodClick != null)
            {
                methodClick.Invoke(sender, parameters);
                hasMethod = true;
            }
        }

        return hasMethod;
    }
}