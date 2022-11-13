using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class TextPresenterScript : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    public TextMeshPro TextMeshPro { get { return _textMeshPro; } }

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }

    public virtual void UpdateAfterTurn()
    {

    }
}
