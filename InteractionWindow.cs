using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class InteractionWindow : MonoBehaviour
{
    private int curSelectSlot;

    private CharacterMgr characterMgr;
    public GameObject characterMgrObj;
    private Goods goods;
    private SoundMgr soundMgr;
    public GameObject goodsObj;
    //text[slot idx][category]
    private Dictionary<int, Dictionary<string, TextMeshProUGUI>> characterStatText = new Dictionary<int, Dictionary<string, TextMeshProUGUI>>();
    private Dictionary<int, Dictionary<string, TextMeshProUGUI>> characterStatPriceText = new Dictionary<int, Dictionary<string, TextMeshProUGUI>>();
    //childs[child name][child name]
    public Dictionary<string, Dictionary<string, Transform>> childs = new Dictionary<string, Dictionary<string, Transform>>();
    private void Start()
    {
        characterMgr = characterMgrObj.GetComponent<CharacterMgr>();
        goods = goodsObj.GetComponent<Goods>();
        for (int winIdx = 0; winIdx < transform.childCount; winIdx++)
        {
            var tmp = new Dictionary<string, Transform>();
            var curWin = transform.GetChild(winIdx);
            for (int i = 0; i < curWin.childCount; i++)
            {
                tmp.Add(curWin.GetChild(i).name, curWin.GetChild(i));
            }
            childs.Add(curWin.name, tmp);
        }
        Setting();
    }
    private void Setting()
    {
        Transform startChild;
        // CharacterList setting
        for (int i = 0; i < childs["Character"]["CharacterList"].childCount; i++)
        {
            startChild = childs["Character"]["CharacterList"].GetChild(i);
            startChild.GetComponent<RawImage>().texture = characterMgr.characters[i].info.texture;
            startChild.GetChild(0).GetComponent<TextMeshProUGUI>().text = characterMgr.characters[i].info.name;
        }
        for (int i = 0; i < childs["Character"]["Slots"].childCount; i++)
        {
            // Bt 접근
            startChild = childs["Character"]["Slots"].GetChild(i).GetChild(0).GetChild(0);
            startChild.GetComponent<RawImage>().texture = characterMgr.characters[i].info.texture;
            startChild.GetChild(0).GetComponent<TextMeshProUGUI>().text = characterMgr.characters[i].info.name;

            // Character 접근
            startChild = childs["Character"]["Slots"].GetChild(i).GetChild(1).GetChild(0);
            startChild.GetComponent<RawImage>().texture = characterMgr.characters[i].info.texture;
            startChild.GetChild(0).GetComponent<TextMeshProUGUI>().text = characterMgr.characters[i].info.name;

            // Stat 접근{
            var statTmp = new Dictionary<string, TextMeshProUGUI>();
            var priceTmp = new Dictionary<string, TextMeshProUGUI>();
            startChild = childs["Character"]["Slots"].GetChild(i).GetChild(1).GetChild(1);
            for (int statIdx = 0; statIdx < startChild.childCount; statIdx++)
            {
                statTmp.Add(startChild.GetChild(statIdx).name, 
                    startChild.GetChild(statIdx).GetChild(1).GetComponent<TextMeshProUGUI>());
                priceTmp.Add(startChild.GetChild(statIdx).name,
                    startChild.GetChild(statIdx).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>());
            }
            characterStatText.Add(i, statTmp);
            characterStatText[i]["HP"].text = characterMgr.characters[i].Stat.HP.ToString();
            characterStatText[i]["AD"].text = characterMgr.characters[i].Stat.AD.ToString();
            characterStatText[i]["AS"].text = characterMgr.characters[i].Stat.AS.ToString();
            characterStatText[i]["HPRecovery"].text = characterMgr.characters[i].Stat.HPRecovery.ToString();
            characterStatText[i]["MPRecovery"].text = characterMgr.characters[i].Stat.MPRecovery.ToString();

            characterStatPriceText.Add(i, priceTmp);
            characterStatPriceText[i]["HP"].text = goods.SelectSlotsPrice(i)["HP"].ToString();
            characterStatPriceText[i]["AD"].text = goods.SelectSlotsPrice(i)["AD"].ToString();
            characterStatPriceText[i]["AS"].text = goods.SelectSlotsPrice(i)["AS"].ToString();
            characterStatPriceText[i]["HPRecovery"].text = goods.SelectSlotsPrice(i)["HPRecovery"].ToString();
            characterStatPriceText[i]["MPRecovery"].text = goods.SelectSlotsPrice(i)["MPRecovery"].ToString();
        }
    } 
    private void BtSound()
    {
        SoundMgr.Instance.PlaySound("bt");
    }
    // 게임 씬으로 전환
    public void StartBt()
    {
        BtSound();
        SceneManager.LoadScene("GameSc");
    }
    // 해당 영역 선택
    public void SelecetWindowBt(string name)
    {
        BtSound();
        for (int i = 0; i < transform.childCount; i++) {
            foreach (Transform child in childs[transform.GetChild(i).name].Values)
            {
                if (transform.GetChild(i).name == name)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    if(child.name != "SelectBt")
                    child.gameObject.SetActive(false);
                }
            }
        }
        ResetSlotsObjActive();
    }
    // 캐릭터 영역
    public void SlotSelectBt(int idx)
    {
        BtSound();
        curSelectSlot = idx;
        foreach (Transform child in childs["Character"].Values)
        {
            if(child.name == "CharacterList")
                child.gameObject.SetActive(false);
        }
        for (int i = 0; i < childs["Character"]["Slots"].childCount; i++)
        {
            Transform startChild = childs["Character"]["Slots"].GetChild(i);
            if (idx == i)
            {
                startChild.gameObject.SetActive(true);
                startChild.GetChild(0).gameObject.SetActive(false);
                startChild.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                startChild.gameObject.SetActive(false);
            }
        }
    }
    public void BuyStatBt(string name)
    {
        BtSound();
        switch (name)
        {
            case "HP":
                if (goods.Pay($"Slot{curSelectSlot + 1}","HP"))
                {
                    characterMgr.characters[curSelectSlot].ApplyChangeState("MaxHP", 
                        characterMgr.characters[curSelectSlot].IncreaseStatValue.HP);
                    characterStatText[curSelectSlot][name].text = characterMgr.characters[curSelectSlot].Stat.HP.ToString();
                    characterStatPriceText[curSelectSlot][name].text = 
                        goods.SelectSlotsPrice(curSelectSlot)["HP"].ToString();
                }
            break;
            case "AD":
                if (goods.Pay($"Slot{curSelectSlot + 1}", "AD"))
                {
                    characterMgr.characters[curSelectSlot].Stat.AD +=
                    characterMgr.characters[curSelectSlot].IncreaseStatValue.AD;
                    characterStatText[curSelectSlot][name].text = characterMgr.characters[curSelectSlot].Stat.AD.ToString();
                    characterStatPriceText[curSelectSlot][name].text =
                        goods.SelectSlotsPrice(curSelectSlot)["AD"].ToString();
                }
                break;
            case "AS":
                if (goods.Pay($"Slot{curSelectSlot + 1}", "AS"))
                {
                    characterMgr.characters[curSelectSlot].ApplyASState(
                        characterMgr.characters[curSelectSlot].IncreaseStatValue.AS);
                    characterStatText[curSelectSlot][name].text = characterMgr.characters[curSelectSlot].Stat.AS.ToString("N2");
                    characterStatPriceText[curSelectSlot][name].text =
                        goods.SelectSlotsPrice(curSelectSlot)["AS"].ToString();
                }
                break;
            case "HPRecovery":
                if (goods.Pay($"Slot{curSelectSlot + 1}", "HPRecovery"))
                {
                    characterMgr.characters[curSelectSlot].Stat.HPRecovery +=
                    characterMgr.characters[curSelectSlot].IncreaseStatValue.HPRecovery;
                    characterStatText[curSelectSlot][name].text = characterMgr.characters[curSelectSlot].Stat.HPRecovery.ToString();
                    characterStatPriceText[curSelectSlot][name].text =
                        goods.SelectSlotsPrice(curSelectSlot)["HPRecovery"].ToString();
                }
                break;
            case "MPRecovery":
                if (goods.Pay($"Slot{curSelectSlot + 1}", "MPRecovery"))
                {
                    characterMgr.characters[curSelectSlot].Stat.MPRecovery +=
                    characterMgr.characters[curSelectSlot].IncreaseStatValue.MPRecovery;
                    characterStatText[curSelectSlot][name].text = characterMgr.characters[curSelectSlot].Stat.MPRecovery.ToString();
                    characterStatPriceText[curSelectSlot][name].text =
                        goods.SelectSlotsPrice(curSelectSlot)["MPRecovery"].ToString();
                }
                break;
        }
    }
    public void ResetSlotsObjActive()
    {
        BtSound();
        for (int i = 0; i < childs["Character"]["Slots"].childCount; i++)
        {
            Transform startChild = childs["Character"]["Slots"].GetChild(i);
            startChild.gameObject.SetActive(true);
            startChild.GetChild(0).gameObject.SetActive(true);
            startChild.GetChild(1).gameObject.SetActive(false);
        }
    }
    // 캐릭터 변경 ( 미구현 )
    public void ChangeCharacterBt()
    {
    }
    // 상점 영역 ( 미구현 )
    public void BuyCrystalW()
    {
    }
    public void BuyCrystalB()
    {
    }
    public void BuyCrystalR()
    {
    }
   
}