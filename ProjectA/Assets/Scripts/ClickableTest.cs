using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTest : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        UnityEngine.Debug.Log("Ow you clicked me");
    }
}
