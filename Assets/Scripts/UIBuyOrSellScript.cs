using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kit.Controls;
using System;

public enum BuyOrSell
{
    Buy,
    Sell
}

public class UIBuyOrSellScript : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private TextMeshProUGUI _buySellTextMeshPro;

    [SerializeField]
    private TextMeshProUGUI _amountTextMeshPro;

    [SerializeField]
    private List<UIButtonScript> _UIButtonScripts;

    private CompanyStock _companyStock;
    private BuyOrSell _buyOrSell;

    private float _maxValue = 0;
    private float _minValue = 0;

    public void Configure(CompanyStock companyStock, BuyOrSell buyOrSell)
    {
        _companyStock = companyStock;
        _buyOrSell = buyOrSell;

        if (buyOrSell == global::BuyOrSell.Buy)
        {
            _maxValue = Mathf.FloorToInt(companyStock.Portfolio.MoneyAmount / companyStock.AmountIntoPrice(1));
            _minValue = 0;

            _buySellTextMeshPro.text = "Buy";
        }
        else
        {
            _maxValue = companyStock.StockAmount;
            _minValue = 0;

            _buySellTextMeshPro.text = "Sell";
        }

        _slider.minValue = _minValue;
        _slider.maxValue = _maxValue;
    }

    private void Action()
    {
        if (ControlsScript.UIControls.BindWithName("Proceed").Down)
        {
            BuyOrSell();
        }
        else if (ControlsScript.UIControls.BindWithName("Cancel").Down)
        {
            Exit();
        }
    }

    private void BuyOrSell()
    {
        if (_buyOrSell == global::BuyOrSell.Buy)
        {
            _companyStock.Buy((int)_slider.value);
        }
        else
        {
            _companyStock.Sell((int)_slider.value);
        }

        ControlsScript.InGameControls.enabled = true;

        Destroy(gameObject);
    }

    private void Exit()
    {
        ControlsScript.InGameControls.enabled = true;

        Destroy(gameObject);
    }

    private void Update()
    {
        Action();

        foreach (UIButtonScript UIButtonScript in _UIButtonScripts)
        {
            if (UIButtonScript.IsMousePositionInside())
            {
                if (ControlsScript.UIControls.BindWithName("Click").Down)
                {
                    if (UIButtonScript.ButtonName == "Proceed")
                    {
                        BuyOrSell();
                    }
                    else if (UIButtonScript.ButtonName == "Cancel")
                    {
                        Exit();
                    }
                }
            }
        }

        _amountTextMeshPro.text = (int)_slider.value + " shares for $" + Math.Round(_companyStock.AmountIntoPrice((int)_slider.value) * 100f / 100f);
    }
}
