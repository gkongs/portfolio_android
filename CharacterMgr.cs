using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMgr : MonoBehaviour // UI 선택에 따라 바뀌도록 코드 변경 필요
{
    private uint slotCount = 3;
    public List<GameObject> slot; // 슬롯은 총 3개임
    public List<Character> characters;
    private StateWindow stateWindow;
    public GameObject stateWindowObj;
    public readonly static float [] SpawnPosX = { -2f, -1.5f,-1f } ; // 슬롯에 따른 생성 위치 
    public float OverallSpeed { get; set; } = 1;

    private void Start()
    {
        characters = new List<Character>();
        stateWindow = stateWindowObj.GetComponent<StateWindow>();
        for (int i = 0; i<slot.Count; i++)
        {
            ObjectPool.Instance.MakeObj(transform, slot[i].gameObject, 1);
            ObjectPool.Instance.Deque(slot[i].name, new Vector2(SpawnPosX[i], transform.position.y));
            characters.Add(transform.GetChild(i).GetComponent<Character>());
            CharactersStateBarSetting(i);
            StartCoroutine(characters[i].ApplyHPRecovery());
        }
    }
    private void CharactersStateBarSetting(int i)
    {
        characters[i].info.stateBar = stateWindow.characterStateBar[i];
        characters[i].info.stateBar.hpBar.maxValue = characters[i].Stat.HP;
        characters[i].info.stateBar.hpBar.value = characters[i].Stat.HP;
    }
    public void NextRoundSettingCharacters()
    {
        for (int i = 0; i < characters.Count; i++)
            characters[i].NextRoundSetting();
    }
    public void AttackAllCharacters()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].Attack();
        }
    }
    public bool DieAllCharacters() // 전체 캐릭이 죽으면 true를 반환하는 함수
    {
        bool _allDie = false;
        for (int i = 0; i < characters.Count; i++)
        {
            _allDie = characters[i].Die;
            if (!_allDie) 
                return false; 
        }
        return true;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                DatabaseMgr.Instance.DBSaveData(characters[i].name, characters[i].Stat);
            }
        }
    }
    private void OnApplicationQuit()
    {
        //Save Stat Data
        for (int i = 0; i < characters.Count; i++)
        {
            DatabaseMgr.Instance.DBSaveData(characters[i].name, characters[i].Stat);
        }
    }
}
