using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour 
{
    [SerializeField] private Text _challengeText;
    [SerializeField]
    private Text _statusText;

    private bool resolved;
    private int resolvePoints;
    private int currentPoints;


    public void SetChallenge(string description, int goal)
	{
        _challengeText.text = description;

        currentPoints = goal;
        resolvePoints = goal;

        _statusText.text = $"{currentPoints} / {resolvePoints}";
	}


    public void DealDamage(int points)
	{
        currentPoints -= points;

        if (currentPoints < 0)
		{
            currentPoints = 0;
		}

        _statusText.text = $"{currentPoints} / {resolvePoints}";
    }


    public bool CheckChallenge()
	{
        return currentPoints == 0;
	}
}
