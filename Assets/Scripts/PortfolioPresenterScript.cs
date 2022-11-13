using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PortfolioPresenterScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _amountTextMeshPro;

    [SerializeField]
    private TextMeshPro _profitTextMeshPro;

    [SerializeField]
    private Portfolio _portfolio;

    public static List<PortfolioPresenterScript> PortfolioPresenterScripts = new List<PortfolioPresenterScript>();

    private float _startingMoney;

    private void Awake()
    {
        PortfolioPresenterScripts.Add(this);
        _startingMoney = _portfolio.MoneyAmount;
    }

    public void UpdateInfo()
    {
        _amountTextMeshPro.text = "$" + Math.Round(_portfolio.MoneyAmount * 100f) / 100f;

        _profitTextMeshPro.text = "$" + Math.Round(_portfolio.Worth() * 100f) / 100f;

        if (_portfolio.Worth() > _startingMoney)
        {
            _profitTextMeshPro.text += " (" + Math.Round((((_portfolio.Worth() / _startingMoney) - 1) * 100) * 100f) / 100f + "%)";
            _profitTextMeshPro.color = Color.green;
        }
        else
        {
            _profitTextMeshPro.text += " (" + Math.Round((((_portfolio.Worth() / _startingMoney) - 1) * 100) * 100f) / 100f + "%)";
            _profitTextMeshPro.color = Color.red;
        }
    }

    public static void Static_UpdateInfo()
    {
        foreach (PortfolioPresenterScript portfolioPresenterScript in PortfolioPresenterScripts)
        {
            portfolioPresenterScript.UpdateInfo();
        }
    }
}
