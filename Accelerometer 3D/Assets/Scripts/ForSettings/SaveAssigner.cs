using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveAssigner : MonoBehaviour
{
    [SerializeField, Tooltip("Slider if that is to be updated too")] private TMP_Dropdown dropdown;
    [Tooltip("The acceleration handler")] private AccelerationHandler accelerationHandler;

    // Called whenever the object becomes enabled or active
    private void OnEnable()
    {
        accelerationHandler = GameObject.FindGameObjectWithTag("RecordingTracker").GetComponent<AccelerationHandler>();
        accelerationHandler.GetSaveAssigner(this);

        AssignSaveSlot(PlayerPrefs.GetInt(gameObject.name, 0));
    }

    public void AssignSaveSlot(int newSaveSlot)
    {
        // Overflow and underflow
        if (newSaveSlot >= dropdown.options.Count)
        {
            newSaveSlot = 0;
        }
        else if (newSaveSlot < 0)
        {
            newSaveSlot = dropdown.options.Count - 1;
        }

        // Update visual
        dropdown.value = newSaveSlot;

        accelerationHandler.ChangeRecordingNumb(newSaveSlot);

        PlayerPrefs.SetInt(gameObject.name, newSaveSlot);
    }
}
