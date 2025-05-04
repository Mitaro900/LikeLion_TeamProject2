using UnityEngine;

public class Playertest : MonoBehaviour
{
    public static Playertest instance;
    public Playertest player;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
