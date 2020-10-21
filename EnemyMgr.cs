using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMgr : MonoBehaviour
{
    private List<List<GameObject>> createEnemies = new List<List<GameObject>>();
    public bool IsSpawn { get; set; }
    public void MakeEnemies(GameObject [] enemies, int size)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            createEnemies.Add(ObjectPool.Instance.MakeObj(transform, enemies[i], size));
        }
    }
    public bool DieAllEnemies()
    {
        if (!IsSpawn)
        {
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).gameObject.activeSelf)
                    return false;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void RoundResetEnemies()
    {
        for (int i = 0; i < createEnemies.Count; i++)
            for(int j =0; j< createEnemies[i].Count; j++)
                ObjectPool.Instance.Enque(createEnemies[i][j]);
    }
    public IEnumerator SpawnEnemy(GameObject [] enemies, int count)
    {
        //몬스터 스폰 루틴....
        //몇 초에 몇 마리씩 어떤 종류로....
        int _rand;
        WaitForSeconds[] delay = {
            new WaitForSeconds(0.3f),
            new WaitForSeconds(0.5f),
            new WaitForSeconds(1f)
        };
        IsSpawn = true; //스폰 중
        for(int i = 0; i < count; i++)
        {
            _rand = Random.Range(0, enemies.Length);
            ObjectPool.Instance.Deque(enemies[_rand].name, transform.position);
            _rand = Random.Range(0, delay.Length);
            yield return delay[_rand];
        }
        IsSpawn = false; //스폰 끝
    }
}
