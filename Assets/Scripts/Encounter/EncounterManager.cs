using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EncounterManager : MonoBehaviour {

    [SerializeField] private int numberOfEncounters = 5;
    [SerializeField] private EncounterDisplay encounterDisplay;
    [SerializeField] private List<EncounterSO> possibleEnemies;
    
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private BaseState currentState;
    private System.Random random;
    private int currentEncounter;


    // States
    public IntroState Dialogue { get; private set; }
    public PlayerTurnState PlayerTurn { get; private set; }
    public EnemyTurnState EnemyTurn { get; private set; }


    private void Awake() {
        random = new System.Random();
        Dialogue = new IntroState(this, enemyAnimator, playerAnimator);
        PlayerTurn = new PlayerTurnState(this);
        EnemyTurn = new EnemyTurnState(this);
        ChangeState(Dialogue);
    }


    private void Update() {
        currentState.UpdateLogic();
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
        int elem = random.Next(0, possibleEnemies.Count);
        encounterDisplay.UpdateEncounter(possibleEnemies[elem]);
        return possibleEnemies[elem];
    }
}
