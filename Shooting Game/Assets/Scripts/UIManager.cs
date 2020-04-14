using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{

    [Header("- Sound Setting")]
    public Slider bgmSilder;
    public Slider sfxSilder;

    public void SwitchBtn(GameObject obj) {
        obj.SetActive(!obj.activeSelf);
    }

    public void UpdateBgmPerText(Text text) {
        text.text = Mathf.Round(bgmSilder.value * 100).ToString() + "%";
    }

    public void UpdateSfxPerText(Text text) {
        text.text = Mathf.Round(sfxSilder.value * 100).ToString() + "%";
    }
}