using System.Collections;
using UnityEngine;

public abstract class BaseState {

    public EncounterManager encounterManager;

    public BaseState(EncounterManager encounterManager) {
        this.encounterManager = encounterManager;
    }

    public virtual void Enter() {
        Debug.Log($"Entering {GetType()}");
    }
    public abstract void UpdateLogic();
    public abstract void Exit();
}
