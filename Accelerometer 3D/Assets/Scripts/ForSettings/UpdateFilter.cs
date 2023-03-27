using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateFilter : MonoBehaviour
{
    [SerializeField, Tooltip("The text component this is to change")] private TextMeshProUGUI textField;
    [SerializeField, Tooltip("Slider if that is to be updated too")] private Slider slider;
    [Tooltip("The acceleration handler")] private AccelerationHandler accelerationHandler;
    [Tooltip("Basic text")] private string baseText;

    // Called whenever the object becomes enabled or active
    private void OnEnable()
    {
        accelerationHandler = GameObject.FindGameObjectWithTag("RecordingTracker").GetComponent<AccelerationHandler>();

        if (baseText == null)
        {
            baseText = textField.text;
        }
        float gottenValue = PlayerPrefs.GetFloat(textField.name, 1f);
        slider.value = gottenValue;
        UpdateFilterValue(gottenValue);
    }

    /// <summary>
    /// Updates text to input value as int
    /// </summary>
    /// <param name="value"></param>
    public void UpdateFilterValue(float value)
    {
        textField.text = string.Format(baseText, (int)value);
        PlayerPrefs.SetFloat(textField.name, value);

        accelerationHandler.ChangeFilter(value);
    }
}
