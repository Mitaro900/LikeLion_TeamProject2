using Singleton.Component;
using UnityEngine;

public class GameManager : SingletonComponent<GameManager>
{
    #region Singleton
    protected override void AwakeInstance()
    {

    }

    protected override bool InitInstance()
    {
        return true;
    }

    protected override void ReleaseInstance()
    {
        
    }
    #endregion
}
