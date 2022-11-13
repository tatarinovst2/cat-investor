using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;

public enum GameMode
{
    News,
    Investing
}

[DefaultExecutionOrder(-20)]
public class MainScript : MonoBehaviour
{
    [SerializeField]
    private List<Company> _companies;
    public List<Company> Companies { get { return _companies; } }

    [SerializeField]
    private int _numberOfArticlesWithCompanyNews = 3;

    [SerializeField]
    private int _otherNewsCount = 1;

    [SerializeField]
    private TextPresenterScript _newsPresenterScript;
    [SerializeField]
    private GameObject _investing;

    [SerializeField]
    private Canvas _canvas;

    private int _turnCount = 1;
    public int TurnCount { get { return _turnCount; } }

    [SerializeField]
    private int _endGameTurn = 6;
    public int EndGameTurn { get { return _endGameTurn; } }

    private GameMode _gameMode = GameMode.News;

    [SerializeField]
    private Portfolio _portfolio;
    public Portfolio Portfolio { get { return _portfolio; } }

    [SerializeField]
    private Portfolio _catPortfolio;
    public Portfolio CatPortfolio { get { return _catPortfolio; } }

    public static MainScript Instance = null;

    [SerializeField]
    private List<TextPresenterScript> _updatableTextPresenterScripts = new List<TextPresenterScript>();

    [SerializeField]
    private List<News> _otherNews;
    private List<News> _usedOtherNews = new List<News>();
    private List<News> _currentOtherNews = new List<News>();

    [SerializeField]
    private List<Event> _events;
    private List<Event> _usedEvents = new List<Event>();

    public void SetGameMode(GameMode gameMode)
    {
        _gameMode = gameMode;

        if (_gameMode == GameMode.News)
        {
            _newsPresenterScript.gameObject.SetActive(true);
            _investing.SetActive(false);
        }
        else
        {
            _newsPresenterScript.gameObject.SetActive(false);
            _investing.SetActive(true);
        }
    }

    private void Awake()
    {
        foreach (Company company in _companies)
        {
            company.Reset();
        }

        Instance = this;
    }

    private void Start()
    {
        foreach (Company company in _companies)
        {
            company.Configure();
            company.UpdateStockPrice();
        }

        List<Company> shuffledCompanies = new List<Company>(_companies);

        for (int i = 0; i < shuffledCompanies.Count; i++)
        {
            Company company = shuffledCompanies[i];

            company.ResetBeforeTurn();

            if (i < _numberOfArticlesWithCompanyNews)
            {
                company.UseRandomNews();
            }
        }

        UseOtherNews();

        CatchNews();

        SetGameMode(GameMode.News);
    }

    public void NextTurn()
    {
        if (_turnCount == _endGameTurn)
        {
            GameObject endGameMenu = CreateMenu("EndGameMenu");

            ControlsScript.InGameControls.enabled = false;

            return;
        }

        CatAI();

        List<Company> shuffledCompanies = new List<Company>(_companies);

        shuffledCompanies.Shuffle<Company>();

        for (int i = 0; i < shuffledCompanies.Count; i++)
        {
            Company company = shuffledCompanies[i];

            company.UpdateStockPrice();

            company.ResetBeforeTurn();

            if (i < _numberOfArticlesWithCompanyNews)
            {
                if (company.UnusedNewsCount() == 0)
                {
                    _numberOfArticlesWithCompanyNews += 1;
                    continue;
                }

                company.UseRandomNews();
            }
        }

        UseOtherNews();

        CatchNews();

        _portfolio.UpdateAfterTurn();
        _catPortfolio.UpdateAfterTurn();

        _turnCount += 1;

        foreach (TextPresenterScript textPresenterScript in  _updatableTextPresenterScripts)
        {
            textPresenterScript.UpdateAfterTurn();
        }

        PortfolioPresenterScript.Static_UpdateInfo();

        TryUseEvent();
    }

    private void TryUseEvent()
    {
        if ((_turnCount == 3) && (_usedEvents.Count != _events.Count))
        {
            GameObject gameObject = CreateMenu("EventMenu");

            UIEventPresenterScript UIEventPresenterScript = gameObject.GetComponent<UIEventPresenterScript>();

            List<Event> _unusedEvents = new List<Event>(_events);

            for (int i = 0; i < _usedEvents.Count; i++)
            {
                _unusedEvents.Remove(_usedEvents[i]);
            }

            int randomEventNumber = Random.Range(0, _unusedEvents.Count);

            UIEventPresenterScript.Configure(_events[randomEventNumber]);

            _usedEvents.Add(_events[randomEventNumber]);

            ControlsScript.InGameControls.enabled = false;
        }
    }

    private void UseOtherNews()
    {
        _currentOtherNews = new List<News>();

        List<News> _shuffledNews = new List<News>(_otherNews);
        _shuffledNews.Shuffle<News>();

        for (int i = 0; i < _usedOtherNews.Count; i++)
        {
            _shuffledNews.Remove(_usedOtherNews[i]);
        }

        for (int i = 0; i < _shuffledNews.Count; i++)
        {
            if (i < _otherNewsCount)
            {
                _currentOtherNews.Add(_shuffledNews[i]);
                _usedOtherNews.Add(_shuffledNews[i]);
            }
        }
    }

    private void CatAI()
    {
        foreach (CompanyStock companyStock in _catPortfolio.CompanyStocks)
        {
            companyStock.Sell(companyStock.StockAmount);
        }

        int randomNumber = Random.Range(0, 6);

        _catPortfolio.CompanyStocks[randomNumber].Buy(_catPortfolio.CompanyStocks[randomNumber].MoneyIntoAmount(_catPortfolio.MoneyAmount));
    }

    private void CatchNews()
    {
        List<string> newsStrings = new List<string>();

        foreach (Company company in _companies)
        {
            if (company.LastUsedNews != null)
            {
                newsStrings.Add(company.LastUsedNews.Text.Replace("{companyName}", company.CompanyName));
            }
        }
        
        foreach (News news in _currentOtherNews)
        {
            newsStrings.Add(news.Text);
        }

        newsStrings.Shuffle<string>();

        string newsString = string.Join("\n\n", newsStrings);

        _newsPresenterScript.TextMeshPro.text = newsString;
    }

    private GameObject CreateMenu(GameObject prefab, Transform parentTransform, Vector2 localPosition = new Vector2())
    {
        GameObject menuObject = Instantiate(prefab);

        menuObject.transform.SetParent(parentTransform, false);
        menuObject.transform.localPosition = localPosition;

        return menuObject;
    }

    public GameObject CreateMenu(GameObject prefab, GameObject parent, Vector2 localPosition = new Vector2())
    {
        return CreateMenu(prefab, parent.transform, localPosition);
    }

    public GameObject CreateMenu(GameObject prefab, Vector2 localPosition = new Vector2())
    {
        return CreateMenu(prefab, _canvas.transform, localPosition);
    }

    public GameObject CreateMenu(string path, GameObject parent, Vector2 localPosition = new Vector2())
    {
        return CreateMenu(Resources.Load<GameObject>(path), parent.transform, localPosition);
    }

    public GameObject CreateMenu(string path, Vector2 localPosition = new Vector2())
    {
        return CreateMenu(Resources.Load<GameObject>(path), _canvas.transform, localPosition);
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
