using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    private Dictionary<string, AudioClip> clipDic = new Dictionary<string, AudioClip>();
    public AudioClip[] clips;
    public static SoundMgr Instance { get; private set; }
    private AudioSource efsSource,bgmSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
        efsSource = GetComponent<AudioSource>();
        //child(0) = bgm obj
        bgmSource = transform.GetChild(0).GetComponent<AudioSource>();
        for (int i = 0; i < clips.Length; i++)
            clipDic.Add(clips[i].name, clips[i]);
    }
    public void PlaySound(string name, bool rand = false)
    {
        if (rand)
        {
            int _rand = Random.Range(1, 3);
            string _playClipName = $"{name + _rand}";
            efsSource.PlayOneShot(clipDic[_playClipName]);
        }
        else
            efsSource.PlayOneShot(clipDic[name]);
    }
    public void BgmOnOff()
    {
        bgmSource.mute = !bgmSource.mute;
    }
    // effect sound 
    public void EfsOnOff()
    {
        efsSource.mute = !efsSource.mute;
    }
}
