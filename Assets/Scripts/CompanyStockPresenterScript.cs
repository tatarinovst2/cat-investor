using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[DefaultExecutionOrder(-10)]
public class CompanyStockPresenterScript : MonoBehaviour
{
    private CompanyStock _companyStock;
    public CompanyStock CompanyStock { get { return _companyStock; } set { _companyStock = value; } }

    [SerializeField]
    private TextMeshPro _companyNameTextMeshPro;
    [SerializeField]
    private TextMeshPro _yourSharesTextMeshPro;
    [SerializeField]
    private TextMeshPro _pricePerShareTextMeshPro;

    [SerializeField]
    private List<BuySellButtonScript> _buySellButtonScripts;

    private void Start()
    {
        UpdateInfo();

        foreach (BuySellButtonScript buySellButtonScript in _buySellButtonScripts)
        {
            buySellButtonScript.Configure(_companyStock);
        }
    }

    public void UpdateInfo()
    {
        _companyNameTextMeshPro.text = _companyStock.Company.CompanyName;
        _yourSharesTextMeshPro.text = _companyStock.StockAmount + " ($" + Math.Round(_companyStock.AmountIntoPrice(_companyStock.StockAmount) * 100f) / 100f + ")";

        string pricePerShareText = "$" + Math.Round(_companyStock.Company.StockPrice * 100f) / 100f;

        if (_companyStock.Company.StockPriceHistory.Count > 1)
        {
            if (_companyStock.Company.StockPrice > _companyStock.Company.StockPriceHistory[_companyStock.Company.StockPriceHistory.Count - 1])
            {
                pricePerShareText +=
                " (" + Math.Round((((_companyStock.Company.StockPrice / _companyStock.Company.StockPriceHistory[_companyStock.Company.StockPriceHistory.Count - 1]) - 1) * 100) * 100f) / 100f + "%)";
                _pricePerShareTextMeshPro.color = Color.green;
            }
            else
            {
                pricePerShareText +=
                " (" + Math.Round((((_companyStock.Company.StockPrice / _companyStock.Company.StockPriceHistory[_companyStock.Company.StockPriceHistory.Count - 1]) - 1) * 100) * 100f) / 100f + "%)";
                _pricePerShareTextMeshPro.color = Color.red;
            }
        }

        _pricePerShareTextMeshPro.text = pricePerShareText;
    }

    public void UpdateAfterTurn()
    {
        UpdateInfo();
    }
}
