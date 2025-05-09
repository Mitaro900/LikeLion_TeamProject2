using System.Collections;
using UnityEngine;

public class BossStageUI : UIBase
{
    enum Images
    {
        pHp0,
        pHp1,
        pHp2,
        pHp3,
        pHp4,
        pHp5,
        bHp0,
        bHp1,
        bHp2,
        bHp3,
        bHp4,
        bHp5,
    }

    private int _playerHp;
    private int _bossHp;
    Vector2[] _playerHpPos = new Vector2[6];
    Vector2[] _bossHpPos = new Vector2[6];

    void Awake()
    {
        BindImage(typeof(Images));
        _playerHpPos[0] = GetImage((int)Images.pHp0).GetComponent<RectTransform>().anchoredPosition;
        _playerHpPos[1] = GetImage((int)Images.pHp1).GetComponent<RectTransform>().anchoredPosition;
        _playerHpPos[2] = GetImage((int)Images.pHp2).GetComponent<RectTransform>().anchoredPosition;
        _playerHpPos[3] = GetImage((int)Images.pHp3).GetComponent<RectTransform>().anchoredPosition;
        _playerHpPos[4] = GetImage((int)Images.pHp4).GetComponent<RectTransform>().anchoredPosition;
        _playerHpPos[5] = GetImage((int)Images.pHp5).GetComponent<RectTransform>().anchoredPosition;

        _bossHpPos[0] = GetImage((int)Images.bHp0).GetComponent<RectTransform>().anchoredPosition;
        _bossHpPos[1] = GetImage((int)Images.bHp1).GetComponent<RectTransform>().anchoredPosition;
        _bossHpPos[2] = GetImage((int)Images.bHp2).GetComponent<RectTransform>().anchoredPosition;
        _bossHpPos[3] = GetImage((int)Images.bHp3).GetComponent<RectTransform>().anchoredPosition;
        _bossHpPos[4] = GetImage((int)Images.bHp4).GetComponent<RectTransform>().anchoredPosition;
        _bossHpPos[5] = GetImage((int)Images.bHp5).GetComponent<RectTransform>().anchoredPosition;
    }

    void DamagedPlayerHp()
    {
        _playerHp--;
        switch (_playerHp)
        {
            case 0: Shoot(GetImage((int)Images.pHp0).GetComponent<RectTransform>(), true); break;
            case 1: Shoot(GetImage((int)Images.pHp1).GetComponent<RectTransform>(), true); break;
            case 2: Shoot(GetImage((int)Images.pHp2).GetComponent<RectTransform>(), true); break;
            case 3: Shoot(GetImage((int)Images.pHp3).GetComponent<RectTransform>(), true); break; 
            case 4: Shoot(GetImage((int)Images.pHp4).GetComponent<RectTransform>(), true); break; 
            case 5: Shoot(GetImage((int)Images.pHp5).GetComponent<RectTransform>(), true); break;
        }
    }

    void DamagedBossHp()
    {
        _bossHp--;
        switch (_playerHp)
        {
            case 0: Shoot(GetImage((int)Images.bHp0).GetComponent<RectTransform>(), false); break;
            case 1: Shoot(GetImage((int)Images.bHp1).GetComponent<RectTransform>(), false); break;
            case 2: Shoot(GetImage((int)Images.bHp2).GetComponent<RectTransform>(), false); break;
            case 3: Shoot(GetImage((int)Images.bHp3).GetComponent<RectTransform>(), false); break;
            case 4: Shoot(GetImage((int)Images.bHp4).GetComponent<RectTransform>(), false); break;
            case 5: Shoot(GetImage((int)Images.bHp5).GetComponent<RectTransform>(), false); break;
        }
    }

    void SetPlayerFullHp()
    {
        _playerHp = 6;
        GetImage((int)Images.pHp0).GetComponent<RectTransform>().anchoredPosition = _playerHpPos[0];
        GetImage((int)Images.pHp1).GetComponent<RectTransform>().anchoredPosition = _playerHpPos[1];
        GetImage((int)Images.pHp2).GetComponent<RectTransform>().anchoredPosition = _playerHpPos[2];
        GetImage((int)Images.pHp3).GetComponent<RectTransform>().anchoredPosition = _playerHpPos[3];
        GetImage((int)Images.pHp4).GetComponent<RectTransform>().anchoredPosition = _playerHpPos[4];
        GetImage((int)Images.pHp5).GetComponent<RectTransform>().anchoredPosition = _playerHpPos[5];
    }
    void SetBossFullHp()
    {
        StopAllCoroutines();
        _bossHp = 6;
        StartCoroutine(BossFullHpCo());
    }

    void Shoot(RectTransform rt, bool isRight)
    {
        StartCoroutine(ShootCo(rt, isRight));
    }

    IEnumerator BossFullHpCo()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        yield return wait;
        GetImage((int)Images.bHp0).GetComponent<RectTransform>().anchoredPosition = _bossHpPos[0];
        yield return wait;
        GetImage((int)Images.bHp1).GetComponent<RectTransform>().anchoredPosition = _bossHpPos[1];
        yield return wait;
        GetImage((int)Images.bHp2).GetComponent<RectTransform>().anchoredPosition = _bossHpPos[2];
        yield return wait;
        GetImage((int)Images.bHp3).GetComponent<RectTransform>().anchoredPosition = _bossHpPos[3];
        yield return wait;
        GetImage((int)Images.bHp4).GetComponent<RectTransform>().anchoredPosition = _bossHpPos[4];
        yield return wait;
        GetImage((int)Images.bHp5).GetComponent<RectTransform>().anchoredPosition = _bossHpPos[5];
    }

    IEnumerator ShootCo(RectTransform rt, bool isRight)
    {
        float initialSpeed = 2000f;
        float launchAngle = isRight ? 30f : 180 - 30f;
        float gravity = -10000f;

        Vector2 pos = rt.anchoredPosition;
        float rad = launchAngle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * initialSpeed;

        while (pos.y >= -1200f)
        {
            velocity.y += gravity * Time.deltaTime;
            pos += velocity * Time.deltaTime;
            rt.anchoredPosition = pos;
            yield return null;
        }
    }
}