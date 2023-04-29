using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter")]
public class Encounter : ScriptableObject 
{
    [SerializeField] private List<Challenge> challenges;
    [SerializeField] private List<EnemyAttack> enemyAttacks;
    [SerializeField] private Sprite enemySprite;

    public List<Challenge> Challenges { get => challenges; set => challenges = value; }
    public List<EnemyAttack> EnemyAttacks { get => enemyAttacks; set => enemyAttacks = value; }
    public Sprite EnemySprite { get => enemySprite; set => enemySprite = value; }
}