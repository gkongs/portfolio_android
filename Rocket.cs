using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectiles
{
    private readonly float range = 0.5f;
    private readonly float groundPosY = 1.1f;
    private Gunner gunner;
    private LayerMask targetLayer;
    private Quaternion orginRot;
    private Vector2 targetPos, myPos, rangeCheckSize, centerPos, targetCheckSize;
    public override Vector2 Direction { get; set; } = Vector2.right;
    public override float Speed { get; set; } = 5;
    public override int Damage { get; set; } = 150;
    public override bool HitCheck(ref Collider2D targetCol)
    {
        if (targetCol.CompareTag("Ground"))
        {
            transform.rotation = Quaternion.identity;
            anim.SetBool("Hit", true);
            SoundMgr.Instance.PlaySound("explosion1");
            Collider2D [] col = Physics2D.OverlapBoxAll(new Vector2(transform.position.x,transform.position.y), 
                rangeCheckSize, 0 , targetLayer);
            for (int i = 0; i < col.Length; i++)
            {
                col[i].gameObject.GetComponent<Enemy>().GetHit(Damage + gunner.Stat.AD);
            }
            enter = true;
            return true;
        }
        return false;
    }
    public override void InitOnEnable()
    {
        Collider2D[] _target = Physics2D.OverlapBoxAll(centerPos, targetCheckSize, 0, targetLayer);
        // 타겟이 하나라도 있을 때
        if (_target.Length != 0) { 
            int _rand = Random.Range(0, _target.Length);
            targetPos = new Vector2(_target[_rand].transform.position.x , groundPosY);
            // 타겟 바라보기
            Vector3 _dir = targetPos - myPos;
            float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        } else
        {
            // ... 타겟이 없을 때
            ObjectPool.Instance.Enque(gameObject);
        }
    }
    public override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
    }
    public override void ReturnObj()
    {
        base.ReturnObj();
        transform.rotation = orginRot;
    }
    public override void Init()
    {
        base.Init();
        gunner = transform.parent.GetComponent<Gunner>();
        targetLayer = 1 << LayerMask.NameToLayer("Enemy");
        myPos = new Vector2(transform.position.x, transform.position.y);
        rangeCheckSize = new Vector2(range, range);
        centerPos = new Vector2(0, 0);
        targetCheckSize = new Vector2(100, 100);
        orginRot = transform.rotation;
    }
}
