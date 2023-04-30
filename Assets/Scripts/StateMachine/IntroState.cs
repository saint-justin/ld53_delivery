using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntroState : BaseState {

    private EncounterSO encounter;
    private Animator enemyAnimator;
    private Animator playerAnimator;

    public IntroState(EncounterManager encounterManager, Animator enemyAnimator, Animator playerAnimator)
        : base(encounterManager) {
        this.enemyAnimator = enemyAnimator;
        this.playerAnimator = playerAnimator;
    }

    public override void Enter() {
        encounter = encounterManager.LoadEncounter();
        enemyAnimator.SetBool("centeredPosition", true);
        playerAnimator.SetBool("centeredPosition", true);
    }

    public override void Exit() {
    }

    public override void UpdateLogic() {
        // TODO Encounter dialogue
        // TODO Show the encounter attacks on Mech 
    }
}
