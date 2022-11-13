using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Result
{
    [SerializeField]
    private string _resultDescription;
    public string ResultDescription { get { return _resultDescription; } }

    [SerializeField]
    private float _probability;
    public float Probability { get { return _probability; } }

    [SerializeField]
    private float _resultMoney;

    public void Use()
    {
        GameObject gameObject = MainScript.Instance.CreateMenu("ResultMenu");

        UIResultPresenterScript UIResultPresenterScript = gameObject.GetComponent<UIResultPresenterScript>();

        UIResultPresenterScript.Configure(this);

        MainScript.Instance.Portfolio.MoneyAmount += _resultMoney;
        PortfolioPresenterScript.Static_UpdateInfo();
    }
}

[System.Serializable]
public class Option
{
    [SerializeField]
    private bool _doingNothingOption;
    public bool DoingNothingOption { get { return _doingNothingOption; } }

    [TextArea]
    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("_resultDescription")]
    private string _optionDescription;
    public string OptionDescription { get { return _optionDescription; } }

    [SerializeField]
    private float _moneyAmountNeeded;
    public float MoneyAmountNeeded { get { return _moneyAmountNeeded; } }

    [SerializeField]
    private List<Result> _results;
    public List<Result> Results { get { return _results; } }

    public void Use()
    {
        float randomNumber = Random.value;

        float probability = 0f;

        for (int i = 0; i < _results.Count; i++)
        {
            probability += _results[i].Probability;

            if (randomNumber <= probability)
            {
                _results[i].Use();
                return;
            }
        }
    }
}

[CreateAssetMenu(menuName="Event")]
public class Event : ScriptableObject
{
    [SerializeField]
    [TextArea]
    private string _eventDescription;
    public string EventDescription { get { return _eventDescription; } }

    [SerializeField]
    private List<Option> _options;
    public List<Option> Options { get { return _options; } }

    public string EventInfo()
    {
        string eventInfo = _eventDescription;

        for (int i = 0; i < _options.Count; i++)
        {
            eventInfo += "\n\nOption " + (i + 1) + ": " + _options[i].OptionDescription;
        }

        return eventInfo;
    }
}
