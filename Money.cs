using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// wrapping (db 데이터 구조 통일)
public class Gold
{
    public Gold(int gold)
    {
        gold = RetainedGold;
    }
    public int RetainedGold; 
}
public class Money
{
    private bool overOneThousand = false;
    
    private static Money instance;
    public static Money Instance
    {
        get
        {
            if (instance == null)
                instance = new Money();
            return instance;
        }
    }
    public Gold Gold { get; set; }
    public TextMeshProUGUI GoldText { get; set; }
    public void ChangeGold(int val)
    {
            Gold.RetainedGold += val;
            ShowText();
    }
    public void ShowText() => GoldText.text = Gold.RetainedGold.ToString();
}
