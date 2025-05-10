using UnityEngine;

public class MessageSign : MonoBehaviour
{
    [SerializeField] private string message;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var ui = UIManager.Instance.GetUI<TutorialUI>();
            if (ui == null) ui = UIManager.Instance.OpenUI<TutorialUI>();

            ui.ShowText(message);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var ui = UIManager.Instance.GetUI<TutorialUI>();
            UIManager.Instance.CloseUI(ui);
        }
    }
}
