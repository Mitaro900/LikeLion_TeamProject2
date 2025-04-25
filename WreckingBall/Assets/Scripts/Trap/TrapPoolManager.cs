using System;
using System.Collections.Generic;
using UnityEngine;

public class TrapPoolManager : MonoBehaviour
{
    public static TrapPoolManager instance;
    [SerializeField] List<int> trapsCount = new();
    [SerializeField] List<GameObject> traps = new();
    Dictionary<string, List<GameObject>> objs = new();

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        objs = new();
        for (int i = 0; i < traps.Count; i++)
        {
            if (trapsCount.Count <= i)
                break;
            string _n = traps[i].name;
            _n = _n.Replace("(Clone)", "");
            if(!objs.ContainsKey(_n))
                objs.Add(_n, new());
            for (int j = 0; j < trapsCount[i]; j++)
            {
                GameObject _t = Instantiate(traps[i], Vector3.zero, Quaternion.identity);
                if (_t.activeSelf)
                    _t.SetActive(false);
                objs[_n].Add(_t);
            }
            
        }
    }

    /// <summary> 오브젝트 불러오기 </summary>
    public List<GameObject> Call(string name, Vector3 pos, int amount = 1)
    {
        if (!objs.ContainsKey(name))
            return null;
        List<GameObject> callObjs = new(amount);
        for (int i=0;i< objs[name].Count;i++)
        {
            if (objs[name][i].activeSelf)
                continue;
            else if (amount <= callObjs.Count)
                return callObjs;
            else
            {
                objs[name][i].transform.position = pos;
                callObjs.Add(objs[name][i]);
            }
        }

        if(callObjs.Count < amount)
        {
            while(callObjs.Count < amount)
            {
                GameObject _o = Instantiate(callObjs[0], pos, Quaternion.identity);
                _o.SetActive(false);
                objs[name].Add(_o);
                callObjs.Add(_o);
            }
        }

        return callObjs;
    }

    /// <summary> 오브젝트 비활성화 메서드 </summary>
    public void Rerurn(GameObject obj)
    {
        //이름
        string _n = obj.name.Replace("Clone", "");

        //신규 오브젝트
        if (!objs.ContainsKey(_n))
            objs.Add(_n, new() { obj });
        //미등록 오브젝트
        else if (!objs[_n].Contains(obj))
            objs[_n].Add(obj);

        //오브젝트 꺼주기
        if(obj.activeSelf)
            obj.SetActive(false);
    }
}
