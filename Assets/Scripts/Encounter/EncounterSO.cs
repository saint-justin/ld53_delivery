using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter")]
public class EncounterSO : ScriptableObject 
{
    [SerializeField] private List<Challenge> challenges;
    [SerializeField] private List<GameObject> enemyAttacks;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private List<string> dialogue; //TBD

    public List<Challenge> Challenges { get => challenges; set => challenges = value; }
    public List<GameObject> EnemyAttacks { get => enemyAttacks; set => enemyAttacks = value; }
    public Sprite EnemySprite { get => enemySprite; set => enemySprite = value; }


    public DamagePattern[] InitialDamage;
    public DamagePattern[] ChallengeDamage1;
    public DamagePattern[] ChallengeDamage2;
    public DamagePattern[] ChallengeDamage3;

    public string[] ChallengeText;

    public int[] Points;
}
