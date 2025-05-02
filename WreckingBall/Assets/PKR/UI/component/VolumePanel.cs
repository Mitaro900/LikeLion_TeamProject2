using System.Collections.Generic;
using UnityEngine;

    public class VolumePanel : MonoBehaviour
    {
        public List<VolumeItem> items;

        public bool CanMove(int row, int dir)
        {
            int newRow = row + dir;
            return newRow >= 0 && newRow < items.Count;
        }

        public void AdjustVolume(int row, int delta)
        {
            items[row].AdjustVolume(delta);
        }

        // SettingUI에서 호출
        public void UpdateHighlight(int selectedRow)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetSelected(i == selectedRow);
            }
        }

        public void Init()
        {
            var bgm = items[0];
            var sfx = items[1];


            bgm.labelText.text = "BGM Volume";
            bgm.slider.value = SoundManager.Instance.BGM.volume * 100;
            bgm.valueText.text = $"{SoundManager.Instance.BGM.volume * 100:N0}%";
            bgm.slider.onValueChanged.AddListener((value) =>
            {
                print("bgm volume changed: " + value);
                SoundManager.Instance.SetBGMVolume(value * 0.01f);
                bgm.valueText.text = $"{SoundManager.Instance.BGM.volume * 100:N0}%";
            });

            sfx.labelText.text = "SFX Volume";
            sfx.slider.value = SoundManager.Instance.SFX.volume* 100;
            sfx.valueText.text = $"{SoundManager.Instance.SFX.volume * 100:N0}%";
            sfx.slider.onValueChanged.AddListener((value) =>
            {
                print("sfx volume changed: " + value);
                SoundManager.Instance.SetSFXVolume(value * 0.01f);
                sfx.valueText.text = $"{SoundManager.Instance.SFX.volume * 100:N0}%";
            });
        }
    }
