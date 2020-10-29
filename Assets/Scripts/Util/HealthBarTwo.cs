using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarTwo : HealthBar
{
    public Transform bar;
    public Image barBox;

    public Color colorHigh;
    public Color colorMid;
    public Color colorLow;

    protected override void SetHealthBar(int hp, int HP)
    {
        //scale the x component
        float frac = (float)hp / HP;

        Vector3 s = bar.transform.localScale;
        s.Set(frac, 1, 1);
        bar.transform.localScale = s;

        /*
        if (frac < .25f) barBox.color = colorLow;
        else if (frac < .50f) barBox.color = colorMid;
        else barBox.color = colorHigh;
        */
    }
}
