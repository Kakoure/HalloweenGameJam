using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : HealthBarTwo
{
    public float secondPhaseHp;
    public Sprite secondPhase;
    public Image img;

    protected override void SetHealthBar(int hp, int HP)
    {
        if (hp <= HP / 2) img.sprite = secondPhase;
        base.SetHealthBar(hp, HP);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
