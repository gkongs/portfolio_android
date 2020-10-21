using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBullet : Projectiles
{
    private WaitForSeconds lifeTime = new WaitForSeconds(2);
    private Healer healer;
    public override Vector2 Direction { get; set; } = Vector2.right;
    public override float Speed { get; set; } = 0.1f;
    public override int Damage { get; set; }
    public override WaitForSeconds LifeTime { get; } = new WaitForSeconds(2);
    public Sprite bullet;
    public override void Init()
    {
        base.Init();
        healer = transform.parent.GetComponent<Healer>();
    }
    public override bool HitCheck(ref Collider2D targetCol)
    {
        if (targetCol.CompareTag("Enemy") && !enter)
        {
            anim.SetBool("Hit", true);
            SoundMgr.Instance.PlaySound("hit", true);
            targetCol.gameObject.GetComponent<Enemy>().GetHit(healer.Stat.AD);
            enter = true;
            return true;
        }
        return false;
    }
    public override void ReturnObj()
    {
        base.ReturnObj();
        GetComponent<SpriteRenderer>().sprite = bullet;
    }
}
