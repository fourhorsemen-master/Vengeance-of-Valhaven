using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomQuote : MonoBehaviour
{
    [SerializeField]
    private Text text = null;
    
    [SerializeField]
    private string[] quotes = null;

    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(_ => SelectRandomQuote());
    }

    private void SelectRandomQuote()
    {
        text.text = $"\"{quotes[Random.Range(0, quotes.Length)]}\"";
    }
}
