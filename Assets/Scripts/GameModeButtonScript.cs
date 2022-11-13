using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeButtonScript : ButtonScript
{
    [SerializeField]
    private GameMode _gameMode;

    protected override void Action()
    {
        MainScript.Instance.SetGameMode(_gameMode);
    }
}
