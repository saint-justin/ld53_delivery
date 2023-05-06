using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour 
{
    [SerializeField]
    private Text _challengeText;
    
    [SerializeField]
    private Text _statusText;

    [SerializeField]
    private Image _defeatIcon;

    private StatType _challengeType;
    public StatType ChallengeType { get { return _challengeType; } }

    private int _value;
    private int _maxValue;


    public void SetChallenge(ChallengeData data)
	{
        _challengeText.text = data.ChallengeText;

        _challengeType = data.ChallengeType;

        _maxValue = data.ChallengeValue;
        _value = data.ChallengeValue;

        _statusText.text = $"{_value} / {_maxValue}";

        _defeatIcon.enabled = false;
	}


    public void DealDamage(StatType type, int points)
	{
        if (type != _challengeType)
		{
            return;
		}

        _value -= points;

        if (_value <= 0)
		{
            _value = 0;
            _defeatIcon.enabled = true;
		}

        _statusText.text = $"{_value} / {_maxValue}";
    }


    public bool CheckChallenge()
	{
        return _value == 0;
	}
}
