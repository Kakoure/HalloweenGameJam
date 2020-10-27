using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    public abstract int Health { set; }
}

public class TextHealthBar : HealthBar
{
    public Text text;
    public string msg = "Health: ";

    public override int Health { set => text.text = msg + value; }
}
