using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Character
{
    public override int SkillCost { get; } = 100;
    public override Stat Stat { get; set; } = new Stat(200, 5, 0.8f, 1, 10);
    public override string Weapon { get; } = "EnergyBullet";
    private CharacterMgr characterMgr;
    public void UseSkill(string name)
    {
        if (name == "Heal")
        {
            ObjectPool.Instance.Deque("HealSkill", transform.position);
        }
        /* 추가 예정
        else if (name == "Buff")
        {
        }
        */
    }
    public override void Init()
    {
        base.Init();
    }
}
