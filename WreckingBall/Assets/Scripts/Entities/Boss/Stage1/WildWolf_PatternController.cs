using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class WildWolf_PatternController : MonoBehaviour
{
    private WildWolf wolf;
    public string patternName { get; private set; } = default;
    public bool isPatternEnd { get; private set; } = true;
    public bool isPatternStart { get; private set; } = false;

    [SerializeField] [ReadOnly]
    private List<StringFloatPair> patternMaxCooldown = new()
    {
        new(nameof(Page1_Pattern1), 0f), new(nameof(Page1_Pattern2), 0f), new(nameof(Page1_Pattern3), 0f),
        new(nameof(Page2_Pattern1), 0f), new(nameof(Page2_Pattern2), 0f), new(nameof(Page2_Pattern3), 0f), new(nameof(Page2_Pattern4), 0f),
    };
    List<StringFloatPair> patternCooldown;

    public void Initialize(WildWolf wildWolf)
    {
        this.wolf = wildWolf;
        PatternInit();
    }

    private void FixedUpdate()
    {
        if (patternCooldown == null)
            patternCooldown = new();
        foreach(var i in patternCooldown)
        {
            if(i.Value > 0f)
                i.Value -= Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        if(wolf.stateMachine.currentState == null || wolf.stateMachine.currentState == wolf.idleState)
        {
            NextAction(true);
        }
    }

    public void NextAction(bool isIgnore = false)
    {
        Debug.Log($"[{patternName}] start: {isPatternStart} / end: {isPatternEnd} / ignore: {isIgnore}");
        if(isIgnore)
            PatternInit(patternName);
        

        if (isPatternStart && !isPatternEnd)
            return;

        string mathod = GetNextPattern();
        Debug.Log(nameof(WildWolf_PatternController) + " " + nameof(NextAction) + " " + nameof(mathod) + " : " + mathod);
        if(mathod != null)
            Invoke(mathod, 0f);
    }

    /// <summary> 다음 패턴 가져오기 </summary>
    /// <returns> return 받은 value 는 Invoke로 시작할것 </returns>
    private string GetNextPattern()
    {
        Debug.Log(nameof(WildWolf_PatternController) + " " + nameof(GetNextPattern) + " " + nameof(wolf.GetBossPage) + " : " + wolf.GetBossPage());
        List<string> actions = new();
        switch (wolf.GetBossPage())
        {
            case 1:

                if(IsPatternCooldown(nameof(Page1_Pattern2)) == false)
                    actions.Add(nameof(Page1_Pattern2));

                if(IsPatternCooldown(nameof(Page1_Pattern3)) == false)
                    actions.Add(nameof(Page1_Pattern3));

                if (actions.Count == 0 || IsPatternCooldown(nameof(Page1_Pattern1)) == false)
                {
                    if(IsPatternCooldown(nameof(Page1_Pattern1)))
                    {
                        int index = patternCooldown.FindIndex(i => i.Key == nameof(Page1_Pattern1));
                        if (index != -1)
                            patternCooldown[index].Value = 0;
                        else
                            patternCooldown.Add(new(nameof(Page1_Pattern1), 0f));
                    }

                    else if(actions.Count == 0)
                        return nameof(Page1_Pattern1);

                    actions.Add(nameof(Page1_Pattern1));
                }

                return actions[Random.Range(0, actions.Count)];

            case 2:

                if(IsPatternCooldown(nameof(Page2_Pattern1)) == false)
                    actions.Add(nameof(Page2_Pattern1));

                if(IsPatternCooldown(nameof(Page2_Pattern2)) == false)
                    actions.Add(nameof(Page2_Pattern2));

                if (IsPatternCooldown(nameof(Page2_Pattern3)) == false)
                    actions.Add(nameof(Page2_Pattern3));

                if(actions.Count == 0 || IsPatternCooldown(nameof(Page2_Pattern4)) == false)
                {
                    if (IsPatternCooldown(nameof(Page2_Pattern4)))
                    {
                        int index = patternCooldown.FindIndex(i => i.Key == nameof(Page2_Pattern4));
                        if (index != -1)
                            patternCooldown[index].Value = 0;
                        else
                            patternCooldown.Add(new(nameof(Page2_Pattern4), 0f));
                    }
                    else if (actions.Count == 0)
                        return nameof(Page2_Pattern4);
                    actions.Add(nameof(Page2_Pattern4));
                }

                return actions[Random.Range(0, actions.Count)];
        }
        return null;
    }

    /// <summary> 현재 쿨타임인지 가져오는 메서드 </summary>
    /// <param name="mathod"> 쿨타임 메서드 이름 </param>
    /// <returns> true:쿨타임O / false:쿨타임X </returns>
    private bool IsPatternCooldown(string mathod)
    {
        if (patternCooldown == null)
        {
            patternCooldown = new() { new(mathod, 0f) };
            return false;
        }

        foreach(var i in patternCooldown)
        {
            if (i.Key == mathod)
                return i.Value > 0f;
        }
        return false;
    }

    private void PatternInit(string mathod = null)
    {
        isPatternStart = false;
        isPatternEnd = true;
        if(mathod != null)
        {
            int index = patternCooldown.FindIndex(i => i.Key == mathod);
            if (index == -1)
                patternCooldown.Add(new(mathod, patternMaxCooldown.Find(i => i.Key == mathod).Value));
            else
                patternCooldown[index].Value = patternMaxCooldown.Find(i => i.Key == mathod).Value;
            Invoke(GetNextPattern(), 0f);
        }
        
    }

    #region Page1
    /// <summary> 멀리 있는 경우,
    /// index 1 : 달리기 시작
    /// index 2 : 달리는 중
    /// index 3 : 공격
    /// index 4 : 달리기 끝
    /// index 5 : 패턴 끝 / 쿨타임
    /// </summary>
    public void Page1_Pattern1()
    {
        string _n = nameof(Page1_Pattern1);
        if (patternName != _n && isPatternStart && !isPatternEnd)
            return;
        patternName = _n;
        isPatternEnd = false;
        isPatternStart = true;
        wolf.stateMachine.ChangeState(wolf.runAttackState);

    }

    /// <summary> ,
    /// index 1 : 바닥쓸기
    /// index 2 : 바닥쓸기
    /// index 3 ? : 바닥쓸기
    /// index 4 : 패턴 끝 / 쿨타임
    /// </summary>
    public void Page1_Pattern2()
    {
        string _n = nameof(Page1_Pattern2);
        if (patternName != _n && isPatternStart && !isPatternEnd)
            return;

        patternName = _n;
        isPatternEnd = false;
        isPatternStart = true;
        wolf.stateMachine.ChangeState(wolf.floorSlideState);
    }

    /// <summary> 벽쪽에 붙은 경우,
    /// index 1 : 트랩 던지기
    /// index 2 : 패턴 끝 / 쿨타임
    /// </summary>
    public void Page1_Pattern3()
    {
        string _n = nameof(Page1_Pattern3);
        if (patternName != _n && isPatternStart && !isPatternEnd)
            return;

        patternName = _n;
        isPatternEnd = false;
        isPatternStart = true;
        wolf.stateMachine.ChangeState(wolf.throwTrapState);
    }
    #endregion

    #region Page2
    
    /// <summary>
    /// index 1 : 벽 오르기
    /// index 2 : (중간에서) 공중쓸기
    /// index 3 : 공중쓸기
    /// index 4 ? : 공중쓸기
    /// index 5 : ----
    /// index 6 : 멈춤 / 쿨타임
    /// </summary>
    public void Page2_Pattern1()
    {
        string _n = nameof(Page2_Pattern1);
        if (patternName != _n && isPatternStart && !isPatternEnd)
            return;

        patternName = _n;
        isPatternEnd = false;
        isPatternStart = true;
        wolf.stateMachine.ChangeState(wolf.aerialSlideState);
    }

    /// <summary>
    /// index 1 : 벽 오르기
    /// index 2 : 트랩 떨구기
    /// index 3 : 통통 튕기기(거의 벽쪽으로 이동)
    /// index 4 : 멈춤 / 쿨타임
    /// </summary>
    public void Page2_Pattern2()
    {
        string _n = nameof(Page2_Pattern2);
        if (patternName != _n && isPatternStart && !isPatternEnd)
            return;
        isPatternEnd = false;
        isPatternStart = true;
        wolf.stateMachine.ChangeState(wolf.droppingTrapState);
    }

    
    /// <summary>
    /// index 1 : 역 V 찍기
    /// index 2 : 역 V 찍기
    /// index 3 : 역 V 찍기
    /// index 4 ? : 역 V 찍기 (거의 벽쪽으로 이동)
    /// index 5 : 멈춤 / 쿨타임
    /// </summary>
    public void Page2_Pattern3()
    {
        string _n = nameof(Page2_Pattern3);
        if (patternName != _n && isPatternStart && !isPatternEnd)
            return;
        isPatternEnd = false;
        isPatternStart = true;
        wolf.stateMachine.ChangeState(wolf.vattackState);
    }

    /// <summary>
    /// index 1 : 달리기 시작
    /// index 2 : 달리는 중
    /// index 3 : 공격
    /// index 4 : 벽 오르기(공격 애니메이션)
    /// index 5 : 벽에서 대각으로 내려찍기
    /// index 6 : 공격
    /// index 7 : 달리기 중(벽에 붙을때 까지)
    /// index 8 : 달리기 끝 / 쿨타임
    /// </summary>
    public void Page2_Pattern4()
    {
        string _n = nameof(Page2_Pattern4);
        if (patternName != _n && isPatternStart && !isPatternEnd)
            return;

        patternName = _n;
        isPatternEnd = false;
        isPatternStart = true;
        wolf.stateMachine.ChangeState(wolf.directAttackState);
    }
    #endregion
}
