using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : MonoBehaviour
{
    private CharacterMgr characterMgr;
    private Character target;
    private Animator anim;
    private SpriteRenderer myRenderer;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        CheckHealTarget();
    }
    public void CheckHealTarget()
    {
        if (characterMgr == null)
            characterMgr = FindObjectOfType<CharacterMgr>();
        float targetHpPer = (float)characterMgr.characters[0].info.curHP / characterMgr.characters[0].Stat.HP;
        target = characterMgr.characters[0];
        for (int i = 1; i < characterMgr.slot.Count; i++)
        {
            // 가장 피가 적은 대상에게 힐 (동일 할 때는 위치 순서상 앞인 대상)
            if ((float)characterMgr.characters[i].info.curHP / characterMgr.characters[i].Stat.HP < targetHpPer
                && !characterMgr.characters[i].Die)
            {
                targetHpPer = (float)characterMgr.characters[i].info.curHP
                    / characterMgr.characters[i].Stat.HP;
                target = characterMgr.characters[i];
            }
        }
        transform.position = target.transform.position + (transform.up * 0.2f);
        anim.SetBool("play", true);
        SoundMgr.Instance.PlaySound("heal");
    }
    public void Heal()
    {
        target.ApplyChangeState("HP", (int)(target.Stat.HP * 0.3));
    }
    public void ReturnObj()
    {
        anim.SetBool("play", false);
        ObjectPool.Instance.Enque(gameObject);
        myRenderer.sprite = null;
    }
}
