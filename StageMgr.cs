using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public struct Stage
{
    public GameObject textObj;
    public GameObject background;
    public GameObject[] enemies;
    public GameObject[] bigWaveEnemies;
    public GameObject[] Boss;
}
public class CurLevel
{
    public int stage { get; set; } = 0;
    public int round { get; set; } = 1;
}
public class StageMgr : MonoBehaviour
{
    private readonly float originNextRoundDist = 100;
    private readonly uint lastRound = 100;
    private readonly WaitForSeconds roundEndCheckDelay = new WaitForSeconds(1f);
    private int resetRound;
    private TextMeshProUGUI text;
    private CharacterMgr characterMgr;
    private EnemyMgr enemyMgr;
    private RectTransform[] backgroundRect = new RectTransform[2]; // 같은 배경 두 장의 위치변경을 통해 bg표현
    private readonly Vector2 changeBackgroundVal = new Vector2(2160, 0);
    private Vector2 moveBackgroundVal;
    private float MoveBackgroundPerFrame;
    public CurLevel curLevel;
    public float NextRoundDist { get; set; }

    public GameObject characterMgr_obj;
    public GameObject esMgr_obj;
    public Stage[] stage;

    private void Init()
    {
        LoadStageData();
        CheckLoadRound();
        // 고정된 캐릭터 - 추후 캐릭터 선택을 통해 변경되는 코드로 수정필요
        characterMgr = characterMgr_obj.GetComponent<CharacterMgr>(); 
        enemyMgr = esMgr_obj.GetComponent<EnemyMgr>();
        NextRoundDist = originNextRoundDist;
        text = stage[curLevel.stage].textObj.GetComponent<TextMeshProUGUI>();
    }
    private void BackgroundSetting()
    {
        //                      X val / 라운드 시작까지 걸리는 프레임 수          
        MoveBackgroundPerFrame = 1080 / (NextRoundDist / characterMgr.OverallSpeed);
        moveBackgroundVal = new Vector2(MoveBackgroundPerFrame, 0);

        backgroundRect[0] = stage[curLevel.stage].background.transform.GetChild(0).GetComponent<RectTransform>();
        backgroundRect[1] = stage[curLevel.stage].background.transform.GetChild(1).GetComponent<RectTransform>();
    }
    // 배경위치 원상복귀
    private void ChangeBackgroundPos()
    {
        if (curLevel.round % 2 == resetRound)
        {
            backgroundRect[0].anchoredPosition += changeBackgroundVal;
        }
        else
            backgroundRect[1].anchoredPosition += changeBackgroundVal;
    }
    private void CheckLoadRound() // 데이터 로드시 홀수 짝수 라운드에 따라 배경 리셋 체인지 
    {
        if (curLevel.round % 2 == 0)
            resetRound = 0;
        else
            resetRound = 1;
    }
    private void ResetBackground()
    {
        RectTransform _tmp = backgroundRect[0];
        backgroundRect[0] = backgroundRect[1];
        backgroundRect[1] = _tmp;
    }
    public void StageStart()
    {
        Init();
        StageSetting();
        StartCoroutine(RoundProcess());
    }
    // 시작 -> 어느정도 거리를 이동한다 -> 적을 조우한다 -> 공격한다 -> 적을 모두 잡거나 죽는다 -> 반복
    IEnumerator RoundProcess()
    {
        while (curLevel.round != lastRound)
        {
            //text 변경
            text.text = $"{curLevel.stage + 1} - {curLevel.round}";
            // round start 
            while (NextRoundDist > 0)
            {
                NextRoundDist = NextRoundDist - characterMgr.OverallSpeed;
                // 배경 무브
                stage[curLevel.stage].background.GetComponent<RectTransform>().anchoredPosition -= moveBackgroundVal;
                yield return null;
            }
            ChangeBackgroundPos();
            // spawn enemies
            SpawnEnemies();
            //characters attack
            characterMgr.AttackAllCharacters();
            //round end
            while (true)
            {
                if (characterMgr.DieAllCharacters())
                {
                    RoundSetting(0);
                    break;
                }
                else if (enemyMgr.DieAllEnemies())
                {
                    RoundSetting(1);
                    break;
                }
                else
                    yield return roundEndCheckDelay;
            }
        }
        StageSetting();
    }
    // 새 Stage 시작시 필요한 작업
    private void StageSetting()
    {
        BackgroundSetting();
        enemyMgr.MakeEnemies(stage[curLevel.stage].enemies, 9);
        enemyMgr.MakeEnemies(stage[curLevel.stage].bigWaveEnemies, 9);
        // 보스 몬스터는 따로.
        enemyMgr.MakeEnemies(stage[curLevel.stage].Boss, 1);
    }
    // 새 Round 시작시 필요한 작업
    // 0 -> 리셋 , 1 -> 라운드 클리어 시
    private void RoundSetting(int i)
    {
        if (i == 0)
        {
            if (curLevel.round % 2 == 1)
                ResetBackground();
            curLevel.round = 1;
            enemyMgr.RoundResetEnemies();
        }
        else
        {
            ++curLevel.round;
        }
        // distance reset
        NextRoundDist = originNextRoundDist;
        characterMgr.NextRoundSettingCharacters();
    }
    private void SpawnEnemies()
    {
        // 3 라운드마다 빅 웨이브
        if (curLevel.round % 3 == 0)
        {
            StartCoroutine(enemyMgr.SpawnEnemy(stage[curLevel.stage].bigWaveEnemies, 9));
        }
        // 5 라운드마다 보스
        if (curLevel.round % 5 == 0)
        {
            StartCoroutine(enemyMgr.SpawnEnemy(stage[curLevel.stage].Boss, 1));
        }
        // 라운드 당 9마리씩 젠 
        StartCoroutine(enemyMgr.SpawnEnemy(stage[curLevel.stage].enemies, 9));
    }
    public bool StageEnd()
    {
        return true;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            DatabaseMgr.Instance.DBSaveData(name, curLevel);
    }
    private void OnApplicationQuit()
    {
        DatabaseMgr.Instance.DBSaveData(name, curLevel);
    }
    private void LoadStageData()
    {
        if (DatabaseMgr.Instance.CheckDBExistUser(name))
            curLevel = DatabaseMgr.Instance.DBLoadData<CurLevel>(name);
        else
            curLevel = new CurLevel();
    }
}
