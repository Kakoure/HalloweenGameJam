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

    public Gradient colorGradient;

    protected override void SetHealthBar(int hp, int HP)
    {
        //scale the x component
        float frac = (float)hp / HP;

        Vector3 s = bar.transform.localScale;
        s.Set(frac, 1, 1);
        bar.transform.localScale = s;

        barBox.color = colorGradient.Evaluate(frac);
    }
}
