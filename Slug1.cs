using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug1 : Enemy
{
    public override int HP { get; set; } = 200;
    public override int AD { get; set; } = 1;
    public override float AS { get; set; } = 0.3f;
    public override float MoveSpeed { get; set; } = 0.3f;
    public override float MoveAnimSpeed { get; set; } = 0.3f;
    public override string MyGold { get; set; } = "EnemyGold";
}
