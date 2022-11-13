using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompanyStock
{
    private int _stockAmount;
    public int StockAmount { get { return _stockAmount; } set { _stockAmount = value; } }

    private Company _company;
    public Company Company { get { return _company; } set { _company = value; } }

    private Portfolio _portfolio;
    public Portfolio Portfolio { get { return _portfolio; } set { _portfolio = value; } }

    public void Buy(int buyAmount)
    {
        _portfolio.MoneyAmount -= AmountIntoPrice(buyAmount);

        _stockAmount += buyAmount;

        foreach (CompanyStockPresenterScript companyStockPresenterScript in _portfolio.CompanyStockPresenterScripts)
        {
            companyStockPresenterScript.UpdateInfo();
        }

        PortfolioPresenterScript.Static_UpdateInfo();
    }

    public void Sell(int sellAmount)
    {
        _portfolio.MoneyAmount += AmountIntoPrice(sellAmount);

        _stockAmount -= sellAmount;

        foreach (CompanyStockPresenterScript companyStockPresenterScript in _portfolio.CompanyStockPresenterScripts)
        {
            companyStockPresenterScript.UpdateInfo();
        }

        PortfolioPresenterScript.Static_UpdateInfo();
    }

    public float AmountIntoPrice(int amount)
    {
        return _company.StockPrice * amount;
    }

    public int MoneyIntoAmount(float money)
    {
        return Mathf.FloorToInt(money / _company.StockPrice);
    }

    public CompanyStock(Company company, Portfolio portfolio)
    {
        _company = company;
        _portfolio = portfolio;
    }
}

[DefaultExecutionOrder(-15)]
public class Portfolio : MonoBehaviour
{
    [SerializeField]
    private float _moneyAmount = 10000f;
    public float MoneyAmount { get { return _moneyAmount; } set { _moneyAmount = value; } }

    private float _startingAmount;
    public float StartingAmount { get { return _moneyAmount; } }

    private List<CompanyStock> _companyStocks = new List<CompanyStock>();
    public List<CompanyStock> CompanyStocks { get { return _companyStocks; } }

    [SerializeField]
    private List<CompanyStockPresenterScript> _companyStockPresenterScripts;
    public List<CompanyStockPresenterScript> CompanyStockPresenterScripts { get { return _companyStockPresenterScripts; } }

    private void Awake()
    {
        _startingAmount = _moneyAmount;
    }

    private void Start()
    {
        foreach (Company company in MainScript.Instance.Companies)
        {
            _companyStocks.Add(new CompanyStock(company, this));
        }

        for (int i = 0; i < _companyStocks.Count; i++)
        {
            if (i < _companyStockPresenterScripts.Count)
            {
                _companyStockPresenterScripts[i].CompanyStock = _companyStocks[i];
            }
        }
    }

    public float Worth()
    {
        float worth = 0f;

        worth += _moneyAmount;

        foreach (CompanyStock companyStock in _companyStocks)
        {
            worth += companyStock.AmountIntoPrice(companyStock.StockAmount);
        }

        return worth;
    }

    public void UpdateAfterTurn()
    {
        for (int i = 0; i < _companyStockPresenterScripts.Count; i++)
        {
            _companyStockPresenterScripts[i].UpdateAfterTurn();
        }
    }
}
