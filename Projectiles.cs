using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectiles : MonoBehaviour
{
    private Enemy enemy;
    protected Animator anim;
    protected bool enter = false;
    public abstract Vector2 Direction { get; set; }
    public abstract float Speed { get; set; }
    private int damage;
    public virtual int Damage { get { return damage; } set { damage = value; } }
    public virtual int Penetration { get; set; } = 1;
    public virtual WaitForSeconds LifeTime { get; } = new WaitForSeconds(5); // 기본 5초
    public virtual void ReturnObj()
    {
        anim.SetBool("Hit", false);
        ObjectPool.Instance.Enque(gameObject);
    }
    public virtual void InitOnEnable() { }
    public virtual void Init()
    {
        anim = GetComponent<Animator>();
    }
    public virtual void Move() => transform.Translate(Direction * Speed);
    public abstract bool HitCheck(ref Collider2D targetCol);

    private void OnEnable()
    {
        enter = false;
        InitOnEnable();
        // 시전 중 적이 없을 경우 오브젝트를 반환하기 때문에 이후 동작 전 널 체크 필요
        if (gameObject.activeSelf)
        {
            StartCoroutine(MoveCheck());
            StartCoroutine(LifeTimeCheck());
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Awake()
    {
        Init();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        HitCheck(ref collision);
    }
    IEnumerator LifeTimeCheck()
    {
        yield return LifeTime;
        ReturnObj();
    }
    IEnumerator MoveCheck()
    {
        while (!anim.GetBool("Hit"))
        {
            Move();
            yield return null;
        }
    }
}
