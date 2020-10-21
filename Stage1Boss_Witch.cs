using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Witch : Enemy
{
    public override int HP { get; set; } = 1000;
    public override int AD { get; set; } = 20;
    public override float AS { get; set; } = 0.2f;
    public override float MoveSpeed { get; set; } = 0.3f;
    public override float MoveAnimSpeed { get; set; } = 0.5f;
    public override string MyGold { get; set; } = "BossGold";
    public override float RANGE => 3;
    public GameObject magicAttack;
    CharacterMgr characterMgr;
    private void Update()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x - 4, transform.position.y));
    }
    public override void Attack()
    {
        for (int i = 0; i < characterMgr.characters.Count; i++)
        {
            if(!characterMgr.characters[i].Die)
                ObjectPool.Instance.Deque("MagicAttack", characterMgr.characters[i].transform.position);
        }
    }
    public override void Init()
    {
        base.Init();
        ObjectPool.Instance.MakeObj(transform, magicAttack, 3);
        characterMgr = FindObjectOfType<CharacterMgr>();
    }
}
