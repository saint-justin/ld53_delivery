using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : BaseState {

    public EnemyTurnState(EncounterManager encounterManager) : base(encounterManager) {
    }

    public override void Enter() {
        base.Enter();
    }

    public override void UpdateLogic() {
    }

    public override void Exit() {
        // TODO Something like:
        //InventoryUI.Instance.FinishTurn();
        // to remove the damage marks
    }
}
