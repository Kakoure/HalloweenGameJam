using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    public abstract void SetHealthBar(int hp, int HP);
}

public class TextHealthBar : HealthBar
{
    public Text text;
    private string msg;

    private void Awake()
    {
        msg = text.text;
    }

    public override void SetHealthBar(int hp, int HP)
    {
        text.text = msg + hp;
    }
}
