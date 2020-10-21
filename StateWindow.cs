using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct CharacterSlider
{
    public Slider hpBar, mpBar;
    public CharacterSlider(Slider hp, Slider mp)
    {
        hpBar = hp;
        mpBar = mp;
    }
}
public class StateWindow : MonoBehaviour
{
    public Dictionary<int, CharacterSlider> characterStateBar { get; set; }
    private void Awake()
    {
        Characterinfo();
    }
    private void Characterinfo()
    {
        characterStateBar = new Dictionary<int, CharacterSlider>();
        Transform startChild = transform.GetChild(0);
        for (int i = 0; i < startChild.childCount; i++)
        {
            characterStateBar.Add(i,new CharacterSlider(startChild.GetChild(i).GetChild(1).GetComponent<Slider>(),
                startChild.GetChild(i).GetChild(2).GetComponent<Slider>()));
        }
    }
}
