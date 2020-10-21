using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectiles
{
    private WaitForSeconds lifeTime = new WaitForSeconds(2);
    public override Vector2 Direction { get; set; } = Vector2.right;
    public override float Speed { get; set; } = 0.2f;
    public override WaitForSeconds LifeTime { get; } = new WaitForSeconds(2);
    private SpriteRenderer myRenderer;
    private Gunner gunner;
    public Sprite sprite;

    public override void Init()
    {
        base.Init();
        gunner = transform.parent.GetComponent<Gunner>();
        myRenderer = GetComponent<SpriteRenderer>();
    }
    public override bool HitCheck(ref Collider2D targetCol)
    {
        if (targetCol.CompareTag("Enemy") && !enter)
        {
            anim.SetBool("Hit", true);
            SoundMgr.Instance.PlaySound("hit", true);
            targetCol.gameObject.GetComponent<Enemy>().GetHit(gunner.Stat.AD);
            enter = true;
            return true;
        }
        return false;
    }
    public override void ReturnObj()
    {
        base.ReturnObj();
        myRenderer.sprite = sprite;
    }
}
