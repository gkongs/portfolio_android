using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_WitchMagic : MonoBehaviour
{
    CharacterMgr characterMgr;
    Stage1Boss_Witch witch;
    private void Awake()
    {
        characterMgr = FindObjectOfType<CharacterMgr>();
        witch = transform.parent.GetComponent<Stage1Boss_Witch>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Character"))
        {
            collision.GetComponent<Character>().GetHit(witch.CurAD);
        }
    }
    public void ReturnObj()
    {
        ObjectPool.Instance.Enque(gameObject);
    }
}
