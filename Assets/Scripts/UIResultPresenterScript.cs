using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;
using TMPro;

public class UIResultPresenterScript : MonoBehaviour
{
    private Result _result;

    [SerializeField]
    private TextMeshProUGUI _textMeshPro;

    [SerializeField]
    private List<UIButtonScript> _UIButtonScripts;

    public void Configure(Result result)
    {
        _result = result;

        _textMeshPro.text = _result.ResultDescription;
    }

    private void Update()
    {
        foreach (UIButtonScript UIButtonScript in _UIButtonScripts)
        {
            if (UIButtonScript.IsMousePositionInside())
            {
                if (ControlsScript.UIControls.BindWithName("Click").Down)
                {
                    ControlsScript.InGameControls.enabled = true;

                    Destroy(gameObject);
                }
            }
        }
    }
}
