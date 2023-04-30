using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : BaseState {

    private EncounterSO encounter;
    private Animator enemyAnimator;
    private Animator playerAnimator;
    private float timeInIntro; // TODO Delete this, temporal var to stay in this state simulating the dialogue

    public IntroState(EncounterManager encounterManager, Animator enemyAnimator, Animator playerAnimator)
        : base(encounterManager) {
        this.enemyAnimator = enemyAnimator;
        this.playerAnimator = playerAnimator;
    }

    public override void Enter() {
        base.Enter();
        encounter = encounterManager.LoadEncounter();
        enemyAnimator.SetBool("centeredPosition", true);
        playerAnimator.SetBool("centeredPosition", true);
    }

    public override void Exit() {
    }

    public override void UpdateLogic() {
        // TODO Encounter dialogue
        // TODO Show the encounter attacks on Mech 
        timeInIntro += Time.deltaTime;
        if (timeInIntro > 3) {
            encounterManager.ChangeState(encounterManager.PlayerTurn);
        }
    }
}
