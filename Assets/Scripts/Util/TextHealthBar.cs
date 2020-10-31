using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    public HealthBar chainLink;

    public void SetHealth(int hp, int HP)
    {
        SetHealthBar(hp, HP);
        chainLink?.SetHealth(hp, HP);
    }
    protected abstract void SetHealthBar(int hp, int HP);
}

public class TextHealthBar : HealthBar
{
    public Text text;
    private string msg;

    private void Awake()
    {
        msg = text.text;
    }

    protected override void SetHealthBar(int hp, int HP)
    {
        text.text = msg + hp;
    }
}
