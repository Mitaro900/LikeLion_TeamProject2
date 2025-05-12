using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PoinItemBase : MonoBehaviour
{
    /// <summary> 비활성화 애니메이션 타입
    /// AnimActiveNextDisable : 애니메이터 활성화시 바로 애니메이션 재생
    /// TriggerActionNextDisable : 기본적으로 애니메이터 활성화, 트리거 disableAnimName로 애니메이션 재생
    /// </summary>
    public enum DisableType
    { AnimActiveNextDisable, TriggerActionNextDisable, }

    [SerializeField] protected DisableType disableType = DisableType.AnimActiveNextDisable;
    [SerializeField] protected string disableAnimName = "Disable";
    [SerializeField] protected AnimationClip disableAnimClip;
    [SerializeField] protected Animator anim;
    [SerializeField] protected int scoreValue;
    public UnityAction disableEvent = null;

    [Tooltip("비활성화 애니메이션 플레이 여부")]
    public bool isDisableAnimActive { get; protected set; } = false;

    public virtual void Start()
    {
        if(anim == null)
            anim = GetComponent<Animator>();
        isDisableAnimActive = false;
        if (anim.enabled && disableType == DisableType.AnimActiveNextDisable)
            anim.enabled = false;
        else if(anim.enabled == false && disableType == DisableType.TriggerActionNextDisable)
            anim.enabled = true;

        //임시
        disableEvent = () => {
            UIManager.Instance.GetUI<NormalStageUI>()?.AddScore(scoreValue);
            UIManager.Instance.GetUI<NormalStageUI>()?.AdjustComboTimer(scoreValue * 0.11f);
        };
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isDisableAnimActive == false)
        {
            isDisableAnimActive = true;
            DisableAnimPlay();
            disableEvent?.Invoke();
        }
    }

    protected void DisableAnimPlay()
    {
        switch(disableType)
        {
            case DisableType.AnimActiveNextDisable:
                anim.enabled = true;
                break;
            case DisableType.TriggerActionNextDisable:
                anim.SetTrigger(disableAnimName);
                break;
        }


        if(disableAnimClip != null)
            Invoke(nameof(DisableAnimEnd), disableAnimClip.length);
        else
        {
            float clipLength = anim.GetCurrentAnimatorClipInfo(0).Length > 0 ? anim.GetCurrentAnimatorClipInfo(0)[0].clip.length : 0f;
            Invoke(nameof(DisableAnimEnd), clipLength);
        }
    }

    protected void DisableAnimEnd()
    {
        anim.enabled = false;
        transform.parent.gameObject.SetActive(false);
    }
}
