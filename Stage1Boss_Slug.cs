using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Slug : Enemy
{
    public override int HP { get; set; } = 1700;
    public override int AD { get; set; } = 15;
    public override float AS { get; set; } = 0.4f;
    public override float MoveSpeed { get; set; } = 0.3f;
    public override float MoveAnimSpeed { get; set; } = 0.3f;
    public override string MyGold { get; set; } = "BossGold";
}
