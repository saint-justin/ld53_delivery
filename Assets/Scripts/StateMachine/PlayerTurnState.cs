using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : BaseState {

    public PlayerTurnState(EncounterManager encounterManager) : base(encounterManager) {
    }

    public override void Enter() {
        base.Enter();
        // Change Inventory state (Different UI panels)
        InventoryUI.Instance.SetInventoryState(InventoryState.Encounter);

        // You can use this to place an overlay showing potential damage for a challenge
        //InventoryUI.Instance.PlaceChallengeDamage(encounterManager.ExampleDamageSO.InitialEffect);
    }

    public override void Exit() {
    }

    public override void UpdateLogic() {
        
    }
}
