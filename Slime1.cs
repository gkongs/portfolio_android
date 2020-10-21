using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime1 : Enemy
{
    private readonly float defalutMoveSpeed = 0.5f;
    public override int HP { get; set; } = 130;
    public override int AD { get; set; } = 8;
    public override float AS { get; set; } = 0.5f;
    public override float MoveSpeed { get; set; }
    public override float MoveAnimSpeed { get; set; } = 0.5f;
    public override string MyGold { get; set; } = "EnemyGold";
    public void MoveStart() => MoveSpeed = defalutMoveSpeed;
    public void MoveStop() => MoveSpeed = 0;
    public override void Init()
    {
        base.Init();
        MyPos = new Vector3(0, (GetComponent<SpriteRenderer>().size.y / 2
            * transform.localScale.y) - 0.08f, 0); //0.01 그림자 사이즈
    }
}
