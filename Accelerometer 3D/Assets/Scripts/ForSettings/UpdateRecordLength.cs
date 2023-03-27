using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateRecordLength : MonoBehaviour
{
    [SerializeField, Tooltip("The text component this is to change")] private TextMeshProUGUI textField;
    [SerializeField, Tooltip("Slider if that is to be updated too")] private TMP_InputField inputField;
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
        float gottenValue = PlayerPrefs.GetFloat(textField.name, 2f);
        UpdateRecordValue(gottenValue);
    }

    /// <summary>
    /// Updates text to input value string as float
    /// </summary>
    /// <param name="value"></param>
    public void UpdateRecordValue(string value)
    {
        float valueAsFloat;

        if (!float.TryParse(value, out valueAsFloat))
        {
            inputField.text = "";
            return;
        }

        UpdateRecordValue(valueAsFloat);
    }

    /// <summary>
    /// Updates text to input value
    /// </summary>
    /// <param name="value"></param>
    public void UpdateRecordValue(float value)
    {
        textField.text = string.Format(baseText, value);
        inputField.text = "";
        PlayerPrefs.SetFloat(textField.name, value);

        accelerationHandler.ChangeRecordLength(value);
    }
}
