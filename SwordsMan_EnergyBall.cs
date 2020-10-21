using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsMan_EnergyBall : Projectiles
{
    private readonly float range = 0.5f;
    private readonly float groundPosY = 1.1f;
    private readonly Vector2 afterEffectScale = new Vector2(2, 2);
    private readonly Vector2 defalutScale = new Vector2(1, 1);
    private Vector2 targetPos, myPos, centerPos, checkRange, checkTarget;
    private LayerMask targetLayer;
    private Swordsman swordsman;
    public bool ThrowBall { get; set; } = false;

    public override Vector2 Direction { get; set; }
    public override float Speed { get; set; } = 1;
    public override int Damage { get; set; } = 450;
    
    public override bool HitCheck(ref Collider2D targetCol)
    {
        if (targetCol.CompareTag("Ground") && !enter)
        {
            transform.rotation = Quaternion.identity;
            anim.SetBool("Hit", true);
            SoundMgr.Instance.PlaySound("explosion2");
            Collider2D[] col = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y),
                checkRange, 0, targetLayer);
            
            for (int i = 0; i < col.Length; i++)
            {
                col[i].gameObject.GetComponent<Enemy>().GetHit(Damage + swordsman.Stat.AD);
            }
            enter = true;
            // Hit후 이펙트 설정
            transform.localScale = afterEffectScale;
            transform.localPosition = new Vector2(transform.localPosition.x, 2);
            return true;
        }
        return false;
    }
    public override void InitOnEnable()
    {
        swordsman.energyBall = this;
        Collider2D[] _target = Physics2D.OverlapBoxAll(centerPos, checkTarget, 0, targetLayer);
        // 타겟이 하나라도 있을 때
        if (_target.Length != 0)
        {
            int _rand = Random.Range(0, _target.Length);
            targetPos = new Vector2(_target[_rand].transform.position.x, groundPosY);
            // 타겟 바라보기
            Vector3 _dir = targetPos - myPos;
            float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            // 스킬 시전 중인지 체크 (라운드 끝 체크)
            StartCoroutine(CheckTarget());
        }
        else
        {
            // ... 타겟이 없을 때
            ObjectPool.Instance.Enque(gameObject);
        }
    }
    IEnumerator CheckTarget()
    {
        while (!transform.parent.GetComponent<Character>().anim.GetBool("move"))
            yield return null;
        ObjectPool.Instance.Enque(gameObject);
    }
    public override void Move()
    {
        if(ThrowBall)
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
    }
    public override void ReturnObj()
    {
        base.ReturnObj();
        transform.localScale = defalutScale;
        ThrowBall = false;
    }
    public override void Init()
    {
        base.Init();
        swordsman = transform.parent.GetComponent<Swordsman>();
        targetLayer = 1 << LayerMask.NameToLayer("Enemy");
        myPos = new Vector2(transform.position.x, transform.position.y);
        checkRange = new Vector2(range, range);
        checkTarget = new Vector2(100, 100);
        centerPos = new Vector2(0, 0);
        ThrowBall = false;
    }
}