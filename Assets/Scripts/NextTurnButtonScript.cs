using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnButtonScript : ButtonScript
{
    protected override void Action()
    {
        MainScript.Instance.NextTurn();
    }
}
