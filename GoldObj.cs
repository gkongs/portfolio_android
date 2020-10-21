using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldObj : MonoBehaviour
{
    private int value;
    private Vector2 target = new Vector2(1.33f, 4.8f); // gold image pos
    private void Awake()
    {
        if (gameObject.name == "EnemyGold")
            value = 100;
        else if (gameObject.name == "BossGold")
            value = 1000;
    }
    private void OnEnable()
    {
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        float time = 0;
        while (time <= 1)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, target, 0.2f);
            yield return null;
        }
        Money.Instance.ChangeGold(value);
        ObjectPool.Instance.Enque(gameObject);
    }
}
