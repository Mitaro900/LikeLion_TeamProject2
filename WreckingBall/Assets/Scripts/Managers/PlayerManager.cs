using Singleton.Component;
using UnityEngine;

public class PlayerManager : SingletonComponent<PlayerManager>
{
    public Player player;

    #region Singleton
    protected override void AwakeInstance()
    {
        Initialize();
    }

    protected override bool InitInstance()
    {
        return true;
    }

    protected override void ReleaseInstance()
    {
        
    }
    #endregion Singleton

    private void OnEnable()
    {
        if (Instance != this)
            Destroy(gameObject);
    }
}
