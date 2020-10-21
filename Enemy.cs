using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private readonly int defalutHPRisePerRound = 10;
    private readonly int defalutADRisePerRound = 3;
    private Ray2D ray; // 공격 범위 체크용
    private Animator anim;
    private LayerMask characterLayerMask;
    private EnemyMgr enemyMgr;
    private Color myColor;
    private StageMgr stageMgr;
    protected SpriteRenderer spriteRenderer;
    public GameObject money;
    public virtual Vector3 MyPos { get; set; }
    public bool Die { get; set; } = false;
    public abstract int HP { get; set; }
    public abstract int AD { get; set; } //attack damage
    public int CurHP { get; set; }
    public int CurAD { get; set; }
    public abstract float AS { get; set; } //attack speed
    public abstract float MoveSpeed { get; set; }
    public abstract float MoveAnimSpeed { get; set; }
    public abstract string MyGold { get; set; }
    public virtual void Move() => transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime); // defalut move
    public virtual float RANGE { get; } = 0.5f;
    public void GetHit(int damage)
    {
        CurHP -= damage;
        if (CurHP <= 0 && !Die)
        {
            Die = true;
            StartCoroutine(AfterDead());
        }
    }
    IEnumerator AfterDead()
    {
        //gold drop
        ObjectPool.Instance.Deque(MyGold, transform.position);
        // gold drop sound play
        SoundMgr.Instance.PlaySound("coin", true);
        // fade out
        while (myColor.a >= 0)
        {
            myColor.a -= 0.1f;
            spriteRenderer.color = myColor;
            yield return null;
        }
        ObjectPool.Instance.Enque(gameObject);
        myColor.a = 1; // 알파 값 복구
        spriteRenderer.color = myColor; //컬러 복구
    }
    IEnumerator MoveCheck() // 수정해야할것같음.
    {
        uint _index = 2; // 가장 오른쪽 캐릭터 스폰 위치 index
        while (!Die)
        {
            if (transform.position.x <= CharacterMgr.SpawnPosX[_index] + RANGE) // 캐릭터 생성 위치에 사거리가 닿을 때
            {
                ray = new Ray2D(new Vector2(transform.position.x, transform.position.y), Vector2.left); // ray 생성
                while (true)
                {
                    if (Physics2D.Raycast(ray.origin, ray.direction, RANGE, characterLayerMask)) // 캐릭터가 있다면
                        ChangeAnim("attack", "move"); // 공격 실행
                    else // 캐릭터가 없다면
                    {
                        ChangeAnim("move", "attack");
                        --_index;
                        if (_index < 0) // 캐릭터를 찾을 수 없을 때.
                        {
                            StopCoroutine(MoveCheck());
                        }
                        break;
                    }
                    yield return null;
                }
            }
            else // 사거리에 닿지 않을 때
                transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime); // 이동
            yield return null;
        }
    }
    public void ChangeAnim(string target, string current)
    {
        anim.SetBool(target, true);
        anim.SetBool(current, false);
    }
    // anim event
    public virtual void Attack()
    { 
        if (Physics2D.Raycast(ray.origin, ray.direction, RANGE, characterLayerMask))
        {
            Physics2D.Raycast(ray.origin, ray.direction, RANGE, characterLayerMask)
             .collider.gameObject.GetComponent<Character>().GetHit(CurAD);
        }
    }
    private void OnEnable()
    {
        Setting();
        StartCoroutine(MoveCheck());
    }
    private void Setting()
    {
        // 생성 위치 조정
        transform.localPosition = MyPos; 
        // 라운드에 따라 능력치 상승
        CurHP = HP + (stageMgr.curLevel.round * defalutHPRisePerRound);
        CurAD = AD + (stageMgr.curLevel.round * defalutADRisePerRound);
        Die = false;
        anim.SetBool("move", true);
        anim.SetFloat("attackSpeed", AS);
        anim.SetFloat("moveSpeed", MoveAnimSpeed);
    }
    private void Awake()
    {
        Init();
    }
    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        myColor = new Color(1, 1, 1, 1);
        stageMgr = FindObjectOfType<StageMgr>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterLayerMask = 1 << LayerMask.NameToLayer("Character");
        MyPos = new Vector3(0, GetComponent<SpriteRenderer>().size.y / 2 * transform.localScale.y, 0);
    }
}
