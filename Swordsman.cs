using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Character
{
    public SwordsMan_EnergyBall energyBall { get; set; }
    public override Stat Stat { get; set; } = new Stat(500, 25, 0.9f, 1, 3);
    public override int SkillCost { get; } = 100;
    public override string Weapon { get; } = "SwordAura";

    public override void Init()
    {
        base.Init();
    }
    public void UseSkill()
    {
        ObjectPool.Instance.Deque("EnergyBall", transform.position + transform.up * 0.5f);
    }
    public void ThrowEnergyBall()
    {
        energyBall.ThrowBall = true;
    }
}
