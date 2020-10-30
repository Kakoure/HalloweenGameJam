using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NoHealthBar : HealthBar
{
    //lol
    protected override void SetHealthBar(int hp, int HP)
    {
        return;
    }
}