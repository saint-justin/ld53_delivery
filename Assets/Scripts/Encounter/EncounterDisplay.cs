using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterDisplay : MonoBehaviour
{
    [SerializeField] private Encounter encounter;
    private List<Challenge> challenges;
    private List<EnemyAttack> enemyAttacks;

    [SerializeField] private Image image;


    private void Awake() 
    {
        UpdateEncounter();
    }

    public void UpdateEncounter() 
    {
        image.sprite = encounter.EnemySprite;
        challenges = encounter.Challenges;
        enemyAttacks = encounter.EnemyAttacks;
    }
}
