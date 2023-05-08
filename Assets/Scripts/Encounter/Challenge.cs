using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour 
{
    [SerializeField]
    private Image _challengeImage;

    [SerializeField]
    private TextMeshProUGUI _challengeTMP;

    [SerializeField]
    private Image _defeatIcon;

    [SerializeField]
    private Image _healthBox;

    [SerializeField]
    private TextMeshProUGUI _healthTMP;

    private StatType _challengeType;
    public StatType ChallengeType { get { return _challengeType; } }

    private int _value;
    private int _maxValue;


	private void Start()
	{
        _challengeImage.alphaHitTestMinimumThreshold = 0.1f;
	}

	public void SetChallenge(ChallengeData data)
	{
        _challengeTMP.text = data.ChallengeText;

        _challengeType = data.ChallengeType;

        _maxValue = data.ChallengeValue;
        _value = data.ChallengeValue;

        _healthTMP.text = $"{_value} / {_maxValue}";

        _defeatIcon.enabled = false;

        if (_challengeType == StatType.Damage)
		{
            _healthBox.color = Color.red;
		}
        else if (_challengeType == StatType.Water)
		{
            _healthBox.color = Color.blue;
		}
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

        _healthTMP.text = $"{_value} / {_maxValue}";
    }


    public bool CheckChallenge()
	{
        return _value == 0;
	}
}
