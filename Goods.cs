using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Price
{
    // category , goodsName 로 value 검색
    public Dictionary<string, int> Slot1StatPrice { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> Slot2StatPrice { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> Slot3StatPrice { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> StoreGoodsPrice { get; set; } = new Dictionary<string, int>();
}

public class Goods : MonoBehaviour
{
    private CharacterMgr characterMgr;
    private int statPrice, highValStatPrice;
    private int crystalPrice = 1000;
    public Price Price { get; set; }
    private void Awake()
    {
        characterMgr = FindObjectOfType<CharacterMgr>();
        statPrice = 50;
        highValStatPrice = 3000;
        LoadPriceData();
    }
    public Dictionary<string,int> SelectSlotsPrice(int idx)
    {
        switch (idx)
        {
            case 0:
                return Price.Slot1StatPrice; 
            case 1:
                return Price.Slot2StatPrice;
            case 2:
                return Price.Slot3StatPrice;
        }
        return null;
    }
    public bool Pay(string category, string goodsName)
    {
        switch (category)
        {
            //값 상승
            case "Slot1":
                if(Money.Instance.Gold.RetainedGold < Price.Slot1StatPrice[goodsName])
                    return false;
                Money.Instance.ChangeGold(-Price.Slot1StatPrice[goodsName]);
                Debug.Log("AAA" + statPrice);
                if (goodsName.Equals("AS") || goodsName.Equals("MPRecovery"))
                    Price.Slot1StatPrice[goodsName] += highValStatPrice;
                else
                    Price.Slot1StatPrice[goodsName] += statPrice;
                break;
            case "Slot2":
                if (Money.Instance.Gold.RetainedGold < Price.Slot2StatPrice[goodsName])
                    return false;
                Money.Instance.ChangeGold(-Price.Slot2StatPrice[goodsName]);
                Debug.Log("AAA" + statPrice);
                if (goodsName.Equals("AS") || goodsName.Equals("MPRecovery"))
                    Price.Slot2StatPrice[goodsName] += highValStatPrice;
                else
                    Price.Slot2StatPrice[goodsName] += statPrice;
                break;
            case "Slot3":
                if (Money.Instance.Gold.RetainedGold < Price.Slot3StatPrice[goodsName])
                    return false;
                Money.Instance.ChangeGold(-Price.Slot3StatPrice[goodsName]);
                if (goodsName.Equals("AS") || goodsName.Equals("MPRecovery"))
                    Price.Slot3StatPrice[goodsName] += highValStatPrice;
                else
                    Price.Slot3StatPrice[goodsName] += statPrice;
                break;
            case "Store":
                if (Money.Instance.Gold.RetainedGold < Price.StoreGoodsPrice[goodsName])
                    return false;
                Money.Instance.ChangeGold(-Price.StoreGoodsPrice[goodsName]);
                Price.StoreGoodsPrice[goodsName] += Price.StoreGoodsPrice[goodsName];
                break;
        }
        return true;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            DatabaseMgr.Instance.DBSaveData(name, Price);
    }
    private void OnApplicationQuit()
    {
        DatabaseMgr.Instance.DBSaveData(name, Price);
    }
    private void LoadPriceData()
    {
        if (DatabaseMgr.Instance.CheckDBExistUser(name))
            Price = DatabaseMgr.Instance.DBLoadData<Price>(name);
        else
        {
            Price = new Price();
            // 캐릭터 스탯
            for (int i = 0; i < 3; i++) {
                SelectSlotsPrice(i).Add("HP", statPrice);
                SelectSlotsPrice(i).Add("AD", statPrice);
                SelectSlotsPrice(i).Add("AS", highValStatPrice);
                SelectSlotsPrice(i).Add("HPRecovery", statPrice);
                SelectSlotsPrice(i).Add("MPRecovery", highValStatPrice);
            }

            // 상점 상품
            Price.StoreGoodsPrice.Add("CrystalW", crystalPrice);
            Price.StoreGoodsPrice.Add("CrystalB", crystalPrice);
            Price.StoreGoodsPrice.Add("CrystalR", crystalPrice);
        }
    }
}