using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private Sprite _hoveringSprite;

    private SpriteRenderer _spriteRenderer;
    private Sprite _defaultSprite;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
    }

    private void CheckMouseClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (ControlsScript.InGameControls.BindWithName("Mouse Left").Down)
                {
                    Action();
                }

                _spriteRenderer.sprite = _hoveringSprite;

                return;
            }
        }

        _spriteRenderer.sprite = _defaultSprite;
    }

    protected abstract void Action();

    private void Update()
    {
        CheckMouseClick();
    }
}
