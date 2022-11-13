using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NewsEffect
{
    Outstanding,
    Good,
    Neutral,
    Undetermined,
    Bad,
    Awful
}

[CreateAssetMenu(menuName="News")]
public class News : ScriptableObject
{
    [SerializeField]
    [TextArea]
    private string _text;
    public string Text { get { return _text; } }

    [SerializeField]
    private NewsEffect _newsEffect;
    public NewsEffect NewsEffect { get { return _newsEffect; } }
}
