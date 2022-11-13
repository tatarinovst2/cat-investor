using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Company")]
public class Company : ScriptableObject
{
    private float _stockPrice = 100f;
    public float StockPrice { get { return _stockPrice; } }

    private List<float> _stockPriceHistory = new List<float>();
    public List<float> StockPriceHistory { get { return _stockPriceHistory; } }

    [SerializeField]
    private string _companyName;
    public string CompanyName { get { return _companyName; } }

    [SerializeField]
    private List<News> _news;

    private List<News> _usedNews = new List<News>();

    private News _lastUsedNews = null;
    public News LastUsedNews { get { return _lastUsedNews; } }

    private NewsEffect _lastNewsEffect = NewsEffect.Neutral;
    public NewsEffect LastNewsEffect { get { return _lastNewsEffect; } }

    public void Configure()
    {
        _stockPrice = Random.Range(10f, 1000f);
    }

    public void UpdateStockPrice()
    {
        _stockPriceHistory.Add(_stockPrice);

        MultipliersBasedOnLastNewsEffect(out var minMultiplier, out var maxMultiplier);

        float multiplier = Random.Range(minMultiplier, maxMultiplier);

        _stockPrice *= multiplier;
    }

    public void MultipliersBasedOnLastNewsEffect(out float minMultiplier, out float maxMultiplier)
    {
        switch (_lastNewsEffect)
        {
            case NewsEffect.Outstanding:
                minMultiplier = 1.5f;
                maxMultiplier = 2f;
                break;

            case NewsEffect.Good:
                minMultiplier = 1.2f;
                maxMultiplier = 2f;
                break;

            case NewsEffect.Undetermined:
                minMultiplier = 0.5f;
                maxMultiplier = 2f;
                break;

            case NewsEffect.Bad:
                minMultiplier = 0.7f;
                maxMultiplier = 0.9f;
                break;

            case NewsEffect.Awful:
                minMultiplier = 0.5f;
                maxMultiplier = 0.7f;
                break;

            default:
                minMultiplier = 0.9f;
                maxMultiplier = 1.1f;
                break;
        }
    }

    public void ResetBeforeTurn()
    {
        _lastNewsEffect = NewsEffect.Neutral;
        _lastUsedNews = null;
    }

    public void UseRandomNews()
    {
        List<News> unusedNews = new List<News>(_news);

        for (int i = 0; i < _usedNews.Count; i++)
        {
            unusedNews.Remove(_usedNews[i]);
        }

        int randomNewsNumber = Random.Range(0, unusedNews.Count);

        _lastUsedNews = unusedNews[randomNewsNumber];
        _lastNewsEffect = unusedNews[randomNewsNumber].NewsEffect;

        _usedNews.Add(unusedNews[randomNewsNumber]);
    }

    public void Reset()
    {
        _stockPrice = 100f;

        _lastNewsEffect = NewsEffect.Neutral;
        _lastUsedNews = null;

        _usedNews = new List<News>();
    }

    public int UnusedNewsCount()
    {
        List<News> unusedNews = new List<News>(_news);

        for (int i = 0; i < _usedNews.Count; i++)
        {
            unusedNews.Remove(_usedNews[i]);
        }

        return unusedNews.Count;
    }
}
