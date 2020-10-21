using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat1 : Enemy
{
    public override int HP { get; set; } = 80;
    public override int AD { get; set; } = 3;
    public override float AS { get; set; } = 0.5f;
    public override float MoveSpeed { get; set; } = 1f;
    public override float MoveAnimSpeed { get; set; } = 0.5f;
    public override string MyGold { get; set; } = "EnemyGold";
}
