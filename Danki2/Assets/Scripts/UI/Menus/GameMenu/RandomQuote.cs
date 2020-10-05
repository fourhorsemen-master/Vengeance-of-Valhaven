using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomQuote : MonoBehaviour
{
    [SerializeField]
    private Text text = null;
    
    [SerializeField]
    private string[] quotes = null;

    private int lastQuoteIndex = 0;

    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            if (gameState == GameState.InGameMenu) SelectRandomQuote();
        });
    }

    private void SelectRandomQuote()
    {
        int newQuoteIndex = Random.Range(0, quotes.Length);
        while (newQuoteIndex == lastQuoteIndex)
        {
            newQuoteIndex = Random.Range(0, quotes.Length);
        }

        lastQuoteIndex = newQuoteIndex;
        
        text.text = $"\"{quotes[lastQuoteIndex]}\"";
    }
}
