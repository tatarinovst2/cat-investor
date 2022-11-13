using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kit.Controls;

public class UIEventPresenterScript : MonoBehaviour
{
    private Event _event;

    [SerializeField]
    private TextMeshProUGUI _textMeshPro;

    [SerializeField]
    private List<UIButtonScript> _UIButtonScripts;

    public void Configure(Event customEvent)
    {
        _event = customEvent;

        _textMeshPro.text = _event.EventInfo();
    }

    private void Update()
    {
        foreach (UIButtonScript UIButtonScript in _UIButtonScripts)
        {
            if (UIButtonScript.IsMousePositionInside())
            {
                if (ControlsScript.UIControls.BindWithName("Click").Down)
                {
                    if (UIButtonScript.ButtonName == "0")
                    {
                        if (_event.Options[0].DoingNothingOption)
                        {
                            Destroy(gameObject);
                            ControlsScript.InGameControls.enabled = true;
                            return;
                        }

                        _event.Options[0].Use();
                        Destroy(gameObject);
                    }
                    else if (UIButtonScript.ButtonName == "1")
                    {
                        if (_event.Options[1].DoingNothingOption)
                        {
                            Destroy(gameObject);
                            ControlsScript.InGameControls.enabled = true;
                            return;
                        }

                        _event.Options[1].Use();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
