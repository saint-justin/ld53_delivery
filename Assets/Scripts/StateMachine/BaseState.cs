using System.Collections;
using UnityEngine;

public abstract class BaseState  {

    public string name;
    public EncounterManager encounterManager;

    public BaseState(EncounterManager encounterManager) {
        this.encounterManager = encounterManager;
    }

    public abstract void Enter();
    public abstract void UpdateLogic();
    public abstract void Exit();
}
