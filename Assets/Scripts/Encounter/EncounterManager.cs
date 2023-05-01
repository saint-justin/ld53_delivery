using System;
using System.Collections;
using System.Collections.Generic;
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
	[SerializeField] private DamageSO _exampleDamageSO;

	private PlayerStats _playerStats;
	public PlayerStats CurrentStats { get { return _playerStats; } }

	private ItemUI currentlySelectedEquipment;

	private Action SelectedAbility;

	[SerializeField]
	private Transform _staticDummy;

	// States
	public IntroState Dialogue { get; private set; }
	public PlayerTurnState PlayerTurn { get; private set; }
	public EnemyTurnState EnemyTurn { get; private set; }
    public DamageSO ExampleDamageSO { get => _exampleDamageSO;}
    public ItemUI CurrentlySelectedEquipment { get => currentlySelectedEquipment; set => currentlySelectedEquipment = value; }

    private void Awake() {
		random = new System.Random();
		Dialogue = new IntroState(this, enemyAnimator, playerAnimator);
		PlayerTurn = new PlayerTurnState(this);
		EnemyTurn = new EnemyTurnState(this);
		currentEncounter = 1;
		//ChangeState(Dialogue);

		if (Instance == null)
		{
			Instance = this;
		}
	}


	private void Start()
	{
		// This is a safe guard to allow testing from the encounter scene directly (not working currently)
		//if (InventoryUI.Instance == null)
		//{
		//	Instantiate(inventoryPrefab);
		//}

		// You can still animate the inventory by controlling the dummy object
		//InventoryUI.Instance.SetFollowTarget(playerAnimator.transform);
		InventoryUI.Instance.SetFollowTarget(_staticDummy);

		//Example();
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

		// Need to implement a way to damage the selected challenge
		Debug.Log($"Dealt {damage} Damage");
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


	// Used by Inventory to set the current Ability
	public void SetSelectedAbility(Action ability)
	{
		SelectedAbility = ability;
	}

	public void UseSelectedAbility()
	{
		if (SelectedAbility != null)
		{
			SelectedAbility.Invoke();
			SelectedAbility = null;
		}
	}


	//public void Select

	#endregion


	#region Test


	private void Example()
	{
		// Change Inventory state (Different UI panels)
		InventoryUI.Instance.SetInventoryState(InventoryState.Encounter);

		// You can use this to place an overlay showing potential damage for a challenge
		InventoryUI.Instance.PlaceChallengeDamage(_exampleDamageSO.InitialEffect);

		// This can be used to apply that damage that was set
		//InventoryUI.Instance.ApplyDamage();

		// Show the first challenge potential damage
		//InventoryUI.Instance.PlaceChallengeDamage(_exampleDamageSO.Challenge1);
	}
	#endregion
}

public struct PlayerStats
{
	public int Energy;
	public int Heat;
	public int Water;
	public int Ammo;
}