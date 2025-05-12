using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadScene(sceneName);
        }
    }
}
