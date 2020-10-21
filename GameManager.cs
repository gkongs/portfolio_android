using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private StageMgr stageMgr;
    public GameObject stageMgr_obj;
    public AudioClip bgm;
    public static int STAGE = 1;
    private void Awake()
    {
        Init();
        GameStart();
    }
    private void Init()
    {
        // 프레임 고정
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        stageMgr = stageMgr_obj.GetComponent<StageMgr>();
    }
    void GameStart()
    {
        // STAGE 시작
        stageMgr.StageStart();
    }
    // 게임 씬으로 전환
    public void StartBt()
    {
        // 로그인 후에만 플레이 가능
        if (DatabaseMgr.Instance.DBLogin)
            SceneManager.LoadScene("GameSc");
    }
    public void LoginBt()
    {
        FirebaseGoogleAuth _firebaseGoogleAuth = FindObjectOfType<FirebaseGoogleAuth>();
        _firebaseGoogleAuth.TryGoogleLogin();
    }
}
