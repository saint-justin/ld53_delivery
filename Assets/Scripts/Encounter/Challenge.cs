using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour 
{
    [SerializeField] private Text text;
    private bool resolved;
    private int resolvePoints;
    private int currentPoints;


    private void Awake() {
        
    }

    public void ChallengeClicked() {
        //EncounterManager.Instance.CurrentlySelectedEquipment.CheckDamage;
        //if (currentPoints >= resolvePoints) {
        //    resolved = true;
        //}
    }


}
