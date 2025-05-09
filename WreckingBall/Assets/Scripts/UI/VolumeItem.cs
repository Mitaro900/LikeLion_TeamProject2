using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeItem : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI labelText;
    public Slider slider;
    public TextMeshProUGUI valueText;
    [SerializeField] private SettingUI settingUI;
    [SerializeField] private Image highlight;
    private int rowIndex;

    public void SetRow(int row)
    {
        rowIndex = row;
    }

    public void SetSelected(bool active)
    {
        highlight.gameObject.SetActive(active);
    }

    public void AdjustVolume(int delta)
    {
        slider.value = Mathf.Clamp(slider.value + delta, 0, 100);
    }

    public void OnPointerEnter(PointerEventData e)
    {
        settingUI.OnVolumeItemHover(rowIndex);
    }
}