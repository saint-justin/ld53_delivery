using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Encounter/DamageSO")]
public class DamageSO : ScriptableObject
{
	public DamagePattern[] InitialEffect;

	public DamagePattern[] Challenge1;

	public DamagePattern[] Challenge2;

	public DamagePattern[] Challenge3;


}
