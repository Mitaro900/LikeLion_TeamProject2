using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NormalStageUI : UIBase
{
    //좌상단: 점수
    //우상단: 콤보
    enum Texts
    {
        ScoreText,
        ComboText,
    }

    enum Images
    {
        ComboBox
    }

    private int _combo;

    void Awake()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        _score = _displayedScore = 0;
        _combo = 0;
        GetText((int)Texts.ComboText).text = "0 combo!";
        GetText((int)Texts.ScoreText).text = "0";
        Image comboBoxImage = GetImage((int)Images.ComboBox);
        _comboBoxOrigin = comboBoxImage.GetComponent<RectTransform>().anchoredPosition;
        _comboBoxTarget = _comboBoxOrigin + new Vector2(0f, -200f);
    }


    #region 스코어

    private int _score;
    private int _displayedScore;
    private float scoreSpeed = 100f;
    private Coroutine scoreCo;

    public void AnimateAddScore(int value)
    {
        _score += value;
        if (scoreCo == null)
            scoreCo = StartCoroutine(AnimateScore());
    }

    public void SetScore(int value)
    {
        if (scoreCo == null) StopCoroutine(scoreCo);
        _score = _displayedScore = value;
        GetText((int)Texts.ScoreText).text = _score.ToString();
    }

    IEnumerator AnimateScore()
    {
        while (true)
        {
            if (_displayedScore == _score) break;
            int newScore = (int)Mathf.MoveTowards(_displayedScore, _score, Mathf.RoundToInt(scoreSpeed * Time.deltaTime));
            _displayedScore = newScore;
            GetText((int)Texts.ScoreText).text = newScore.ToString();
            yield return null;
        }

        scoreCo = null;
    }

    #endregion

    #region 콤보

    //콤보박스의 상태: 대기, 등장중, 광고중, 사라지는중,
    private enum ComboBoxState
    {
        Idle,
        Showing,
        Advertising,
        Hiding
    }

    private ComboBoxState _comboBoxState = ComboBoxState.Idle;

    private Coroutine comboCo;
    private Vector2 _comboBoxOrigin;
    private Vector2 _comboBoxTarget;

    public void SetCombo(int value)
    {
        _combo = value;
        GetText((int)Texts.ComboText).text = $"{_combo} combo!";

        // 콤보박스가 사라지거나 대기 상태이면 새로 등장 애니메이션을 실행
        if (_comboBoxState == ComboBoxState.Idle || _comboBoxState == ComboBoxState.Hiding)
        {
            if (comboCo != null) StopCoroutine(comboCo);
            comboCo = StartCoroutine(ShowComboBox());
        }
        // 이미 등장 중이거나 광고 대기 중이면 광고 대기 시간을 갱신(재시작)
        else if (_comboBoxState == ComboBoxState.Advertising)
        {
            if (comboCo != null) StopCoroutine(comboCo);
            comboCo = StartCoroutine(AdvertisementWait());
        }
    }


    IEnumerator ShowComboBox()
    {
        _comboBoxState = ComboBoxState.Showing;
        Image comboBoxImage = GetImage((int)Images.ComboBox);
        RectTransform rt = comboBoxImage.GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;
        Vector2 targetPos = _comboBoxTarget;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            yield return null;
        }

        rt.anchoredPosition = targetPos;
        yield return AdvertisementWait();
    }

    IEnumerator AdvertisementWait()
    {
        _comboBoxState = ComboBoxState.Advertising;
        yield return new WaitForSeconds(3f);
        comboCo = StartCoroutine(HideComboBox());
    }

    IEnumerator HideComboBox()
    {
        _comboBoxState = ComboBoxState.Hiding;
        Image comboBoxImage = GetImage((int)Images.ComboBox);
        RectTransform rt = comboBoxImage.GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;
        Vector2 targetPos = _comboBoxOrigin;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            yield return null;
        }

        rt.anchoredPosition = targetPos;
        _comboBoxState = ComboBoxState.Idle;
        comboCo = null;
    }

    #endregion
}