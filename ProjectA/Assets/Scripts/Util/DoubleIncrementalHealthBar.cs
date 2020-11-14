using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//Ignores maxHp
public class DoubleIncrementalHealthBar : HealthBar
{
    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    protected override void SetHealthBar(int hp, int HP)
    {
        for (int i = 1; i <= hearts.Length; i++)
        {
            int hpVal = 2 * i - 1;
            if (hp < hpVal)
            {
                hearts[i - 1].sprite = emptyHeart;
            }
            else if (hp == hpVal)
            {
                hearts[i - 1].sprite = halfHeart;
            }
            else
            {
                hearts[i - 1].sprite = fullHeart;
            }
        }
    }
}
