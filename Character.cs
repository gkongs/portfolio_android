using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UseObject
{
    public GameObject obj;
    public int size;
}
[System.Serializable]
public struct Info {
    public int curHP;
    public int curMP;
    public string name;
    public Texture2D texture;
    public UseObject[] useObjects;
    public CharacterSlider stateBar;
}
// 스탯 증가량
public struct IncreaseStatValue
{
    public IncreaseStatValue(int HP, int AD, float AS, int HPRecovery, int MPRecovery)
    {
        this.HP = HP;
        this.AD = AD;
        this.AS = AS;
        this.HPRecovery = HPRecovery;
        this.MPRecovery = MPRecovery;
    }
    public int HP;
    public int AD;
    public float AS;
    public int HPRecovery;
    public int MPRecovery;
}
public class Stat
{
    public Stat(int HP, int AD, float AS, int HPRecovery, int MPRecovery)
    {
        this.HP = HP;
        this.AD = AD;
        this.AS = AS;
        this.HPRecovery = HPRecovery;
        this.MPRecovery = MPRecovery;
    }
    public int HP { get; set; }
    public int AD { get; set; }
    public float AS { get; set; }
    public int HPRecovery { get; set; }
    public int MPRecovery { get; set; }
}
public abstract class Character : MonoBehaviour
{
    private bool resetAnim = false;
    private readonly int RANGE = 15; //고정 사거리
    private Ray2D ray; // 공격 범위 체크용
    private LayerMask targetLayerMask;
    private WaitForSeconds hpRecoveryDelay = new WaitForSeconds(1); // 체력 회복 주기
    public Animator anim;
    public GameObject objmgr;
    public Info info;
    public abstract Stat Stat { get; set; }
    public abstract int SkillCost { get; }
    public abstract string Weapon { get; }
    public virtual IncreaseStatValue IncreaseStatValue { get; } = new IncreaseStatValue(10, 1, 0.05f, 1, 1);
    public bool Die { get; set; } = false;
    public virtual string CurBehavior { get; set; }
    
    // 공격 시 마나 획득
    public void AttackMeans()
    {
        info.curMP += Stat.MPRecovery;
        info.stateBar.mpBar.value += Stat.MPRecovery;
        ObjectPool.Instance.Deque(Weapon , transform.position);
    }
    public virtual void Attack()
    {
        //거리에 따른 공격 판정
        StartCoroutine(EnemyCheck());
        //스킬 사용 -> 공격을 하다가 마나가 차면 스킬을 사용하는 방식
        StartCoroutine(SkillCheck());
    }
    public void GetHit(int damage)
    {
        ApplyChangeState("HP", -damage);
        if (info.curHP <= 0 && !Die)
        {
            Die = true;
            anim.SetBool("die", true);
        }
    }
    public void AfterDead()
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void ChangeAnim(string target, string current)
    {
        anim.SetBool(target, true);
        anim.SetBool(current, false);
    }
    public void NextRoundSetting()
    {
        StopAllCoroutines();
        StartCoroutine(ApplyHPRecovery());
        if (Die)
        {
            transform.GetComponent<BoxCollider2D>().enabled = true;
            ApplyChangeState("HP", Stat.HP);
            ApplyChangeState("MP", -info.curMP);
            Die = false;
        }
        anim.SetBool("move", true);
        anim.SetBool("die", false);
        anim.SetBool("attack", false);
        anim.SetBool("skill", false);
    }
    IEnumerator EnemyCheck()
    {
        while (true)
        {
            if (Physics2D.Raycast(ray.origin, ray.direction, RANGE, targetLayerMask))
            {
                ChangeAnim("attack", "move");
                break;
            }
            yield return null;
        }
    }
    IEnumerator SkillCheck()
    {
        while (true)
        {
            if (SkillCost <= info.curMP)
            {
                ApplyChangeState("MP", -SkillCost);
                ChangeAnim("skill", "attack"); // 스킬이 전 캐릭 1개라는 가정하에 아니라면 각자 캐릭 별로 루틴이 필요.
                yield return null;
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
                ChangeAnim("attack", "skill");
            }
            yield return null;
        }
    }
    public IEnumerator ApplyHPRecovery() {
        while (!Die)
        {
            ApplyChangeState("HP", Stat.HPRecovery);
            yield return hpRecoveryDelay;
        }
    }
    public void ApplyChangeState(string type, int changeVal)
    {
        switch (type)
        {
            case "HP":
                if (info.curHP + changeVal > Stat.HP)
                    info.curHP = Stat.HP;
                else if (info.curHP + changeVal < 0)
                    info.curHP = 0;
                else
                    info.curHP += changeVal;
                info.stateBar.hpBar.value = info.curHP;
                break;
            case "MaxHP":
                Stat.HP += changeVal;
                info.curHP += changeVal;
                info.stateBar.hpBar.maxValue = Stat.HP;
                info.stateBar.hpBar.value = info.curHP;
                break;
            case "MP":
                info.curMP += changeVal;
                info.stateBar.mpBar.value = info.curMP;
                break;
            case "ALL":
                info.curHP += changeVal;
                info.curMP += changeVal;
                info.stateBar.hpBar.value = info.curHP;
                info.stateBar.mpBar.value = info.curMP;
                break;
        }
    }
    public void ApplyASState(float changeVal)
    {
        Stat.AS += changeVal;
        anim.SetFloat("attackSpeed", Stat.AS); // 애니메이션에 공속 적용 
    }
    private void Awake()
    {
        Init();
    }
    public virtual void Init()
    {
        LoadData();
        anim = GetComponent<Animator>();
        ray = new Ray2D(new Vector2(transform.position.x,
            transform.position.y), Vector2.right);
        targetLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        // object 생성
        for (int i = 0; i < info.useObjects.Length; i++)
            ObjectPool.Instance.MakeObj(transform, info.useObjects[i].obj, info.useObjects[i].size);
        anim.SetFloat("attackSpeed", Stat.AS);
        // 현재 체력, 마나
        info.curHP = Stat.HP;
        info.curMP = 0;
    }
    private void LoadData()
    {
        if (DatabaseMgr.Instance.CheckDBExistUser(name))
        {
            Stat = DatabaseMgr.Instance.DBLoadData<Stat>(name);
        }
    }
}
