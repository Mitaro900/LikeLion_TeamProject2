using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] Color selectedColor = Color.magenta;
    [SerializeField] Color deSelectedColor= Color.white;
    public Action onClicked;
    public Action onSelected;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSelected(bool isSelected)
    {
        text.color = isSelected ? selectedColor : deSelectedColor;
    }

    public void SetClickEffect()
    {
        StartCoroutine(flickerCo());
    }

    IEnumerator flickerCo()
    {
        Color magenta = selectedColor;
        Color white = deSelectedColor;

        text.color = white;
        yield return new WaitForSeconds(0.1f);
        text.color = magenta;
        yield return new WaitForSeconds(0.1f);
        text.color = white;
        yield return new WaitForSeconds(0.1f);
        text.color = magenta;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onSelected?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClicked?.Invoke();
    }
}