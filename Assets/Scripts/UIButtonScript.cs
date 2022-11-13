using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;
using UnityEngine.UI;

public class UIButtonScript : MonoBehaviour
{
    [SerializeField]
    private string _buttonName;
    public string ButtonName { get { return _buttonName; } }

    private Image _background;

    [SerializeField]
    private Sprite _hoveringSprite;

    private Sprite _defaultSprite;

    private void Awake()
    {
        _background = GetComponent<Image>();
        _defaultSprite = _background.sprite;
    }

    public bool IsMousePositionInside()
    {
        Vector2 mousePosition = ControlsScript.MousePosition();

        if ((mousePosition.x < _background.transform.position.x + (_background.rectTransform.sizeDelta.x / 2f)) &&
           (mousePosition.x > _background.transform.position.x - (_background.rectTransform.sizeDelta.x / 2f)) &&
           (mousePosition.y < _background.transform.position.y + (_background.rectTransform.sizeDelta.y / 2f)) &&
           (mousePosition.y > _background.transform.position.y - (_background.rectTransform.sizeDelta.y / 2f)))
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (IsMousePositionInside())
        {
            _background.sprite = _hoveringSprite;
        }
        else
        {
            _background.sprite = _defaultSprite;
        }
    }
}
