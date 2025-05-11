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

    enum Sliders
    {
        Slider
    }

    void Awake()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindSlider(typeof(Sliders));

        _score = 0;
        _combo = 0;
        GetText((int)Texts.ComboText).text = "0 combo!";
        GetText((int)Texts.ScoreText).text = "0";
        Image comboBoxImage = GetImage((int)Images.ComboBox);
        _comboBoxOrigin = comboBoxImage.GetComponent<RectTransform>().anchoredPosition;
        _comboBoxTarget = _comboBoxOrigin + new Vector2(0f, -200f);
    }

    #region 스코어

    private int _score;

    public void AddScore(int value)
    {
        _score += value;
        GetText((int)Texts.ScoreText).text = _score.ToString();
    }

    #endregion

    #region 콤보

    //콤보박스의 상태: 대기, 등장중, 광고중, 사라지는중,
    private enum ComboBoxState
    {
        Idle,
        Appearing,
        Displaying,
        Hiding
    }

    private ComboBoxState _comboBoxState = ComboBoxState.Idle;

    private int _combo;
    private float _comboTimer;
    private const float _comboDuration = 6.7f; // 콤보박스가 사라지기까지의 시간
    private Coroutine comboCo;
    private Vector2 _comboBoxOrigin;
    private Vector2 _comboBoxTarget;

    public void AddCombo()
    {
        _combo++;
        GetText((int)Texts.ComboText).text = $"{_combo} combo!";
        // 콤보박스가 사라지거나 대기 상태이면 새로 등장 애니메이션을 실행
        if (_comboBoxState == ComboBoxState.Idle || _comboBoxState == ComboBoxState.Hiding)
        {
            if (comboCo != null) StopCoroutine(comboCo);
            comboCo = StartCoroutine(AppearComboBox());
        }
        // 이미 등장 중이면 광고 대기 시간을 갱신(재시작)
        else if (_comboBoxState == ComboBoxState.Displaying)
        {
            if (comboCo != null) StopCoroutine(comboCo);
            comboCo = StartCoroutine(DisplayComboBox());
        }
    }

    public void AdjustComboTimer(float seconds)
    {
        _comboTimer += seconds;
        Mathf.Clamp(_comboTimer, 0, _comboDuration);
    }

    private IEnumerator AppearComboBox()
    {
        _comboBoxState = ComboBoxState.Appearing;
        Image comboBoxImage = GetImage((int)Images.ComboBox);
        RectTransform rt = comboBoxImage.GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;
        Vector2 targetPos = _comboBoxTarget;
        float duration = 0.35f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            yield return null;
        }

        rt.anchoredPosition = targetPos;
        yield return DisplayComboBox();
    }

    private IEnumerator DisplayComboBox()
    {
        _comboBoxState = ComboBoxState.Displaying;
        _comboTimer = _comboDuration;
        GetSlider((int)Sliders.Slider).value = 1;

        while (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
            GetSlider((int)Sliders.Slider).value = _comboTimer / _comboDuration;
            yield return null;
        }

        AddScore((int)(Mathf.Pow(_combo, 2) / 0.25f) + _combo * 10);
        _combo = 0;

        comboCo = StartCoroutine(HideComboBox());
    }

    private IEnumerator HideComboBox()
    {
        _comboBoxState = ComboBoxState.Hiding;
        Image comboBoxImage = GetImage((int)Images.ComboBox);
        RectTransform rt = comboBoxImage.GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;
        Vector2 targetPos = _comboBoxOrigin;
        float duration = 0.35f;
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