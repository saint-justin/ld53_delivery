using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour {

	[SerializeField] private int numberOfEncounters = 5;
	[SerializeField] private EncounterDisplay encounterDisplay;
	[SerializeField] private List<EncounterSO> possibleEnemies;
	
	[SerializeField] private Animator enemyAnimator;
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private BaseState currentState;
	private System.Random random;
	private int currentEncounter;

	[SerializeField] private GameObject inventoryPrefab;
	public static EncounterManager Instance;

	private EncounterSO _currentEncounterSO;

	private PlayerStats _playerStats;
	public PlayerStats CurrentStats { get { return _playerStats; } }

	private ItemUI currentlySelectedEquipment;

	private Action SelectedAbility;
	private StatType SelectedAbilityType;

	private int _selectedChallenge;

	[SerializeField]
	private Transform _staticDummy;

	[SerializeField]
	private Challenge[] _challenges;

	[SerializeField]
	private GameObject _challengePanel;

	[SerializeField]
	private GameObject _scorePanel;

	[SerializeField]
	private TextMeshProUGUI _endScore;

	// States
	public IntroState Dialogue { get; private set; }
	public PlayerTurnState PlayerTurn { get; private set; }
	public EnemyTurnState EnemyTurn { get; private set; }
    public ItemUI CurrentlySelectedEquipment { get => currentlySelectedEquipment; set => currentlySelectedEquipment = value; }

    private void Awake() {
		random = new System.Random();
		Dialogue = new IntroState(this, enemyAnimator, playerAnimator);
		PlayerTurn = new PlayerTurnState(this);
		EnemyTurn = new EnemyTurnState(this);
		currentEncounter = 0;
		//ChangeState(Dialogue);

		if (Instance == null)
		{
			Instance = this;
		}
	}


	private void Start()
	{
		// You can still animate the inventory by controlling the dummy object
		//InventoryUI.Instance.SetFollowTarget(playerAnimator.transform);
		InventoryUI.Instance.SetFollowTarget(_staticDummy);

		_challengePanel.SetActive(false);
		_scorePanel.SetActive(false);
	}


	


	private void Update() {
		//currentState.UpdateLogic();
	}

	#region StateMachine

	public void ChangeState() {
		if (currentState.GetType() == typeof(IntroState)) {
			ChangeState(PlayerTurn);
		} else if (currentState.GetType() == typeof(PlayerTurnState)) {
			ChangeState(EnemyTurn);
		} else if (currentState.GetType() == typeof(EnemyTurnState)) { 
			if (currentEncounter >= numberOfEncounters) {
				SceneManager.LoadScene("InventoryTestScene");
			} else {
				currentEncounter++;
				ChangeState(Dialogue);
			}
		}
	}

	public void ChangeState(BaseState newState) {
		currentState?.Exit();
		currentState = newState;
		currentState.Enter();
	}


	private void HandleEncounter() {
		// Instantiate all states for this encounter -> here
		// Load encounter -> in IntroState
		// Encounter dialogue -> in IntroState
		// Show the encounter attacks on Mech -> in IntroState
		// Player action -> PlayerTurn
		// Resolve challenges
		// Heat/time added
		// Rearrange components
	}

	/// <summary>
	/// Loads a random encounter from the list os ScriptableObjects that can be added via inspector
	/// </summary>
	public EncounterSO LoadEncounter() {
		int elem = random.Next(0, possibleEnemies.Count - 1);
		encounterDisplay.UpdateEncounter(possibleEnemies[elem]);
		return possibleEnemies[elem];
	}

	#endregion

	// These are used by Player Action Callbacks that can be selected on items to handle encounter side logic
	// Most of the inventory logic is already handled but things like changing Player stats are not

	#region PlayerAction


	public void DealDamage(ItemUI item)
	{
		// AbilityValue is a generic parameter thats use depends on the item type
		// In this case used to select which die range to use
		int dieChoice = item.ItemSO.AbilityValue;

		int damage = UtilityFunctions.RollDice(dieChoice);

		_challenges[_selectedChallenge].DealDamage(StatType.Damage, damage);
	}


	public void MoveSpaces(ItemUI item)
	{
		// Actual movement is implemented by the inventory
	}


	public void ApplyShield(ItemUI item)
	{
		// Shield placement is handled by inventory
	}


	public void GenerateEnergy(ItemUI item)
	{
		// This may be a passive effect will have to ask
	}


	public void HeatSink(ItemUI item)
	{
		// This may be a passive effect or only used in between encounters
	}


	public void SprayWater(ItemUI item)
	{
		_challenges[_selectedChallenge].DealDamage(StatType.Water, 1);
	}


	// Used by Inventory to set the current Ability
	public void SetSelectedAbility(StatType abilityType, Action ability)
	{
		SelectedAbility = ability;
		SelectedAbilityType = abilityType;
	}

	#endregion


	#region Test

	public void EndEncounter()
	{
		if (_currentEncounterSO.ChallengesData[0].EffectType == StatType.Damage)
		{
			InventoryUI.Instance.ApplyDamage();
		}

		
		for (int i = 1; i < _currentEncounterSO.ChallengesData.Length; i++)
		{
			if (!CheckChallenge(i-1))
			{
				ApplyEffects(i, true);
			}
		}


		// Heat Check
		int heatRoll = UnityEngine.Random.Range(0, 100);

		if (heatRoll < _playerStats.Heat)
		{
			InventoryUI.Instance.PlaceChallengeDamage(_currentEncounterSO.HeatDamage, Vector2.zero, false, true, true);
			InventoryUI.Instance.ApplyDamage();
		}

		InventoryUI.Instance.SetTimeAndHeat(_playerStats.Time, _playerStats.Heat);

		InventoryUI.Instance.RemoveDamagedItems();

		InventoryUI.Instance.SetInventoryState(InventoryState.Rearrange);
	}


	private bool CheckChallenge(int index)
	{
		return _challenges[index].CheckChallenge();
	}


	private void ApplyEffects(int index, bool applyDamage)
	{
		if (_currentEncounterSO.ChallengesData[index].EffectType == StatType.Damage)
		{
			InventoryUI.Instance.PlaceChallengeDamage(_currentEncounterSO.ChallengesData[index].DamagePatterns, _currentEncounterSO.ChallengesData[index].DamagePos, !applyDamage, true, false);
			
			if (applyDamage)
			{
				InventoryUI.Instance.ApplyDamage();
			}
			
		}
		else if (_currentEncounterSO.ChallengesData[index].EffectType == StatType.Heat)
		{
			_playerStats.Heat += _currentEncounterSO.ChallengesData[index].EffectValue;
		}
		else if (_currentEncounterSO.ChallengesData[index].EffectType == StatType.Time)
		{
			_playerStats.Time += _currentEncounterSO.ChallengesData[index].EffectValue;
		}
		else if (_currentEncounterSO.ChallengesData[index].EffectType == StatType.Cargo)
		{
			InventoryUI.Instance.DestroyCheapestCargo();
		}
	}


	public void NextEncounter()
	{
		currentEncounter++;

		if (currentEncounter < possibleEnemies.Count)
		{
			LoadEncounter(currentEncounter);
		}
		else
		{
			EndScreen();
		}
	}

	public void LoadEncounter(int index)
	{
		_challengePanel.SetActive(true);


		if (index >= possibleEnemies.Count)
		{
			Debug.LogError("Error");
			return;
		}


		_currentEncounterSO = possibleEnemies[index];

		if (_currentEncounterSO == null)
		{
			Debug.LogError("Error");
			return;
		}

		for (int i = 1; i < _currentEncounterSO.ChallengesData.Length; i++)
		{
			_challenges[i-1].gameObject.SetActive(true);
			_challenges[i-1].SetChallenge(_currentEncounterSO.ChallengesData[i]);
		}

		for (int i = _currentEncounterSO.ChallengesData.Length; i < 4; i++)
		{
			_challenges[i-1].gameObject.SetActive(false);
		}

		// Change Inventory state (Different UI panels)
		InventoryUI.Instance.SetInventoryState(InventoryState.Encounter);


		ApplyEffects(0, false);

		InventoryUI.Instance.SetTimeAndHeat(_playerStats.Time, _playerStats.Heat);
	}


	public void SelectChallenge(int i)
	{
		_selectedChallenge = i;

		if (SelectedAbility != null && SelectedAbilityType == _challenges[i].ChallengeType)
		{
			SelectedAbility.Invoke();
		}
	}


	public void EndScreen()
	{
		InventoryUI.Instance.SetInventoryState(InventoryState.Hidden);

		_challengePanel.SetActive(false);

		_scorePanel.SetActive(true);

		ItemTally tally = InventoryUI.Instance.TallyItemAttributes();

		_endScore.text = $"Final Score: {tally.Value}";
	}

	#endregion
}

public struct PlayerStats
{
	public int Energy;
	public int Heat;
	public int Water;
	public int Ammo;
	public int Time;
}