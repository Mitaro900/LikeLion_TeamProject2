using Singleton.Component;
using UnityEngine;

public class PlayerManager : SingletonComponent<PlayerManager>
{
    public Player player;

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
}
