using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct GoldType {
    public GameObject enemy;
    public GameObject boss;
}
public class GoldMgr : MonoBehaviour
{
    public GameObject retainedGold;
    public GoldType goldType;
    private void Awake()
    {
        Money.Instance.GoldText = retainedGold.GetComponent<TextMeshProUGUI>();
        LoadGoldData();
        ObjectPool.Instance.MakeObj(transform, goldType.enemy, 20);
        ObjectPool.Instance.MakeObj(transform, goldType.boss, 5);
    }
    private void LoadGoldData()
    {
        if (DatabaseMgr.Instance.CheckDBExistUser(name))
            Money.Instance.Gold = DatabaseMgr.Instance.DBLoadData<Gold>(name);
        else
            Money.Instance.Gold = new Gold(100);
        Money.Instance.ShowText();
    }
    private void OnApplicationPause(bool pause)
    {
        if(pause)
            DatabaseMgr.Instance.DBSaveData(name, Money.Instance.Gold);
    }
    private void OnApplicationQuit()
    {
        DatabaseMgr.Instance.DBSaveData(name, Money.Instance.Gold);
    }
}
