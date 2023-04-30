using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EncounterManager : MonoBehaviour {

    [SerializeField] private int numberOfEncounters = 5;
    [SerializeField] private EncounterDisplay encounterDisplay;
    [SerializeField] private List<EncounterSO> possibleEnemies;
    private System.Random random;
    private int currentEncounter;
    private BaseState currentState;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private Animator playerAnimator;


    // States
    private IntroState dialogue;
    private PlayerTurnState playerTurn;
    private EnemyTurnState enemyTurn;


    private void Awake() {
        random = new System.Random();
        dialogue = new IntroState(this, enemyAnimator, playerAnimator);
        playerTurn = new PlayerTurnState(this);
        enemyTurn = new EnemyTurnState(this);
        ChangeState(dialogue);
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
