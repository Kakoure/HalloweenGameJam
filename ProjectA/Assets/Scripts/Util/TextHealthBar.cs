using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    [Tooltip("Extra health bar that is liked to this one")]
    public HealthBar linkedHealthBar;

    public void SetHealth(int hp, int HP)
    {
        SetHealthBar(hp, HP);
        linkedHealthBar?.SetHealth(hp, HP);
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
