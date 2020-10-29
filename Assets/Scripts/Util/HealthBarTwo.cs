using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HealthBarTwo : HealthBar
{
    public Transform bar;

    public override void SetHealthBar(int hp, int HP)
    {
        //scale the x component
        float frac = (float)hp / HP;

        Vector3 s = bar.transform.localScale;
        s.Set(frac, 1, 1);
        bar.transform.localScale = s;
    }
}
