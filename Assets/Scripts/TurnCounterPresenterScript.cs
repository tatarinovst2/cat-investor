using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCounterPresenterScript : TextPresenterScript
{
    public override void UpdateAfterTurn()
    {
        TextMeshPro.text = "TURN " + MainScript.Instance.TurnCount;
    }
}
