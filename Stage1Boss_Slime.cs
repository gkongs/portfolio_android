using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Slime : Enemy
{
    private readonly float defalutMoveSpeed = 0.7f;
    public override int HP { get; set; } = 1300;
    public override int AD { get; set; } = 15;
    public override float AS { get; set; } = 0.7f;
    public override float MoveSpeed { get; set; }
    public override float MoveAnimSpeed { get; set; } = 0.3f;
    public override string MyGold { get; set; } = "BossGold";
    public void MoveStart() => MoveSpeed = defalutMoveSpeed;
    public void MoveStop() => MoveSpeed = 0;
}
