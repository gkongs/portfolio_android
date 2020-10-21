using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Character
{
    public override int SkillCost { get; } = 100;
    public override string Weapon { get; } = "Bullet";
    public override Stat Stat { get; set; } = new Stat(300, 20, 1, 1, 5);
    public uint MaxRocket { get; set; } = 3; // 일단. 
    public void UseSkill()
    {
        StartCoroutine(CreateRocket());
    }
    IEnumerator CreateRocket()
    {
        WaitForSeconds _delay = new WaitForSeconds(0.3f);
        int _count = 0;
        while (_count < MaxRocket)
        {
            ++_count;
            ObjectPool.Instance.Deque("Rocket", transform.position + transform.up * 10);
            yield return _delay;
        }
    }
}
