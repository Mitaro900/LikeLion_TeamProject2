using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public Player player;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}

