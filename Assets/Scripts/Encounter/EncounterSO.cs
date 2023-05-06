using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter")]
public class EncounterSO : ScriptableObject 
{
    //[SerializeField] private List<Challenge> challenges;
    //[SerializeField] private List<GameObject> enemyAttacks;
    //[SerializeField] private Sprite enemySprite;
    //[SerializeField] private List<string> dialogue; //TBD

    //public List<Challenge> Challenges { get => challenges; set => challenges = value; }
    //public List<GameObject> EnemyAttacks { get => enemyAttacks; set => enemyAttacks = value; }
    //public Sprite EnemySprite { get => enemySprite; set => enemySprite = value; }

    public ChallengeData[] ChallengesData;

    public DamagePattern[] HeatDamage;
}

[System.Serializable]
public struct ChallengeData
{
    public string ChallengeText;
    public StatType ChallengeType;
    public int ChallengeValue;

    public bool AvoidDamagedSlots;
    public bool IsHeatDamage;
    public Vector2 DamagePos;
    public DamagePattern[] DamagePatterns;

    public StatType EffectType;
    public int EffectValue;
}


public enum StatType
{
    None,
    Damage,
    Heat,
    Water,
    Time,
    Ammo,
    Cargo
}