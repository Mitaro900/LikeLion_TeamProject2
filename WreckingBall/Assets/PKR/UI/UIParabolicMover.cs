using UnityEngine;
using System.Collections;

public class UIParabolicMover : MonoBehaviour
{
    [Header("References")]
    // 움직일 UI 오브젝트의 RectTransform
    public RectTransform target;

    [Header("Launch Parameters")]
    // 발사 초기 속도 (UI 픽셀/초 단위)
    public float initialSpeed = 2000f;
    // 발사 각도 (도 단위)
    public float launchAngle = 30f;
    // 중력 가속도 (UI 픽셀/초² 단위, 음수 값)
    public float gravity = -10000f;

    [Header("Simulation Settings")]
    // 시뮬레이션 종료를 위한 바닥 높이 (시작 높이에 상대적)
    public float groundOffset = -1200f;

    // 내부 상태
    private Vector2 _startPos;
    private bool _isShooting = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Launch();
        }
    }
    // 발사 호출 메서드
    public void Launch()
    {
        if (_isShooting) return;
        _startPos = target.anchoredPosition;
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        _isShooting = true;

        // 발사 초기 위치와 속도 벡터 계산
        Vector2 pos = _startPos;
        float rad = launchAngle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * initialSpeed;

        // 목표로 하는 바닥 높이
        float groundY = _startPos.y + groundOffset;

        // 시뮬레이션 루프: 바닥 높이 이하로 내려올 때까지
        while (pos.y >= groundY)
        {
            // 속도에 중력 가속도 적용
            velocity.y += gravity * Time.deltaTime;
            // 위치 업데이트
            pos += velocity * Time.deltaTime;
            target.anchoredPosition = pos;

            yield return null;
        }

        // 최종 위치 보정 (바닥에 딱 맞춰 찍히도록)
        pos.y = groundY;
        target.anchoredPosition = pos;

        _isShooting = false;
    }
}