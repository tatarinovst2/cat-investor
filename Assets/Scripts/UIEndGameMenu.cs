using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;
using UnityEngine.SceneManagement;
using TMPro;

public class UIEndGameMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _winnerTextMeshPro;

    [SerializeField]
    private TextMeshProUGUI _amountTextMeshPro;

    [SerializeField]
    private List<UIButtonScript> _UIButtonScripts;

    private void Start()
    {
        float catWorth = MainScript.Instance.CatPortfolio.Worth();
        float playerWorth = MainScript.Instance.Portfolio.Worth();

        if (playerWorth > catWorth)
        {
            _winnerTextMeshPro.text = "Game ended!\nYou won!";
        }
        else
        {
            _winnerTextMeshPro.text = "Game ended!\nYou lost!";
        }

        _amountTextMeshPro.text = "Your potrfolio's worth: " + playerWorth;
    }

    private void Update()
    {
        if (ControlsScript.UIControls.BindWithName("Proceed").Down)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (ControlsScript.UIControls.BindWithName("Cancel").Down)
        {
            Application.Quit();
        }

        foreach (UIButtonScript UIButtonScript in _UIButtonScripts)
        {
            if (UIButtonScript.IsMousePositionInside())
            {
                if (ControlsScript.UIControls.BindWithName("Click").Down)
                {
                    if (UIButtonScript.ButtonName == "Proceed")
                    {
                        SceneManager.LoadScene("SampleScene");
                    }
                    else if (UIButtonScript.ButtonName == "Cancel")
                    {
                        Application.Quit();
                    }
                }
            }
        }
    }
}
