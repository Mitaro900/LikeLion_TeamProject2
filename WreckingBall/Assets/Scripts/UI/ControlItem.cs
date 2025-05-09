using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlItem : MonoBehaviour
{
    public BindingManager.Action action;
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI bindingButtonText;
    [SerializeField] private SettingUI settingUI;
    [SerializeField] private Image highlightRebind;
    [SerializeField] private Image highlightReset;
    private int rowIndex;

    public void SetRow(int row)
    {
        rowIndex = row;
    }

    public void SetSelected(bool isRow, int selCol)
    {
        highlightRebind.gameObject.SetActive(isRow && selCol == 0);
        highlightReset.gameObject.SetActive(isRow && selCol == 1);
    }

    public void OnPointerEnterKey()
    {
        settingUI.OnControlItemHover(rowIndex, 0);
    }

    public void OnPointerEnterReset()
    {
        settingUI.OnControlItemHover(rowIndex, 1);
    }

    public void StartRebind()
    {
        settingUI.StartRebind(action);
    }

    public void ResetKey()
    {
        settingUI.ResetKey(action);
    }
}