using System;
using System.Collections;
using UnityEngine;

public class TutorialUI : UIBase
{
    [SerializeField] private float typingSpeed = 0.05f;

    enum GameObjects
    {
        Box,
    }

    enum Texts
    {
        descText,
    }

    void Awake()
    {
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        GetObject((int)GameObjects.Box).SetActive(false);
    }

    

    Coroutine textCo = null;

    public void ShowText(string text)
    {
        if (textCo != null) StopCoroutine(textCo);
        textCo = StartCoroutine(TextCo(text));
    }

    IEnumerator TextCo(string contents)
    {
        GetObject((int)GameObjects.Box).SetActive(true);
        GetText((int)Texts.descText).text = "";
        var wait = new WaitForSeconds(typingSpeed);
        for (int i = 0; i < contents.Length; i++)
        {
            GetText((int)Texts.descText).text += contents[i];
            yield return wait;
        }

        yield return new WaitForSeconds(1.5f);
        GetObject((int)GameObjects.Box).SetActive(false);
        GetText((int)Texts.descText).text = "";
    }
}