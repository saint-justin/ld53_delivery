using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterDisplay : MonoBehaviour { 

    [SerializeField] private EncounterSO encounter;
    private List<Challenge> challenges;
    private List<GameObject> enemyAttacks;

    [SerializeField] private Image image;


    private void Awake() {
    }

    public void UpdateEncounter(EncounterSO newEncounter) {
        //encounter = newEncounter;
        //image.sprite = encounter.EnemySprite;
        //challenges = encounter.Challenges;
        //enemyAttacks = encounter.EnemyAttacks;
    }
}
