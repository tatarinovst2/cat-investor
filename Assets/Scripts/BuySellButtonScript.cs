using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;

public class BuySellButtonScript : ButtonScript
{
    [SerializeField]
    private BuyOrSell _buyOrSell;

    private CompanyStock _companyStock;

    public void Configure(CompanyStock companyStock)
    {
        _companyStock = companyStock;
    }

    protected override void Action()
    {
        GameObject buySellMenu = MainScript.Instance.CreateMenu("BuySellMenu");

        ControlsScript.InGameControls.enabled = false;

        UIBuyOrSellScript UIBuyOrSellScript = buySellMenu.GetComponent<UIBuyOrSellScript>();

        if (_buyOrSell == BuyOrSell.Buy)
        {
            UIBuyOrSellScript.Configure(_companyStock, BuyOrSell.Buy);
        }
        else
        {
            UIBuyOrSellScript.Configure(_companyStock, BuyOrSell.Sell);
        }
    }
}
