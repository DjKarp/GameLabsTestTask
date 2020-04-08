using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Смена текста на кнопках при нажатии
/// </summary>
public class GUIController : MonoBehaviour
{

    private GameObject toggleOnOffWaves;
    private TextMeshProUGUI toggleOnOffWavesTMP;

    private GameObject toggleOnOffScroll;
    private TextMeshProUGUI toggleOnOffScrollTMP;

    private GameObject GPUtoggleOnOffWaves;
    private TextMeshProUGUI GPUtoggleOnOffWavesTMP;

    private GameObject GPUtoggleOnOffScroll;
    private TextMeshProUGUI GPUtoggleOnOffScrollTMP;


    private void Awake()
    {

        toggleOnOffWaves = GameObject.Find("ToggleOnOffWaves");
        toggleOnOffWavesTMP = toggleOnOffWaves.GetComponentInChildren<TextMeshProUGUI>();

        toggleOnOffScroll = GameObject.Find("ToggleOnOffScroll");
        toggleOnOffScrollTMP = toggleOnOffScroll.GetComponentInChildren<TextMeshProUGUI>();

        GPUtoggleOnOffWaves = GameObject.Find("ToggleOnOffWavesGPU");
        GPUtoggleOnOffWavesTMP = GPUtoggleOnOffWaves.GetComponentInChildren<TextMeshProUGUI>();

        GPUtoggleOnOffScroll = GameObject.Find("ToggleOnOffScrollGPU");
        GPUtoggleOnOffScrollTMP = GPUtoggleOnOffScroll.GetComponentInChildren<TextMeshProUGUI>();

    }

    public void SetToggleNameOnButtonToggleWaves(bool isOn)
    {

        if (isOn) toggleOnOffWavesTMP.text = "Wave OFF";
        else toggleOnOffWavesTMP.text = "Wave On";

    }

    public void SetToggleNameOnButtonToggleScroll(bool isOn)
    {

        if (isOn) toggleOnOffScrollTMP.text = "Scroll Texture OFF";
        else toggleOnOffScrollTMP.text = "Scroll Texture ON";

    }

    public void SetToggleNameOnButtonToggleWavesGPU(bool isOn)
    {

        if (isOn) GPUtoggleOnOffWavesTMP.text = "Wave OFF";
        else GPUtoggleOnOffWavesTMP.text = "Wave On";

    }

    public void SetToggleNameOnButtonToggleScrollGPU(bool isOn)
    {

        if (isOn) GPUtoggleOnOffScrollTMP.text = "Scroll Texture OFF";
        else GPUtoggleOnOffScrollTMP.text = "Scroll Texture ON";

    }

}
