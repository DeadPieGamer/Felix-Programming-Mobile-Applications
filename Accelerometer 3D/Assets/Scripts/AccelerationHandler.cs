using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccelerationHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Textfield used for countdown")] private TextMeshProUGUI textField;
    [Tooltip("Base text to fill the textField")] private string baseText;

    [SerializeField, Tooltip("How many measurements are used to calculate a mean"), Range(1, 10)] private int filterLevel = 1;

    [Tooltip("Counting up to filter")] private int currentFilter = 0;
    [Tooltip("Current sum acceleration")] private float filterAcceleration = 0f;

    [Tooltip("Whether acceleration should be recorded at the moment")] private bool recordAcceleration = false;
    [Tooltip("float used to store the total acceleration")] private List<Vector2> sumAcceleration;
    [Tooltip("float used to store the recording beginning time")] private float recordStart;
    [Tooltip("The recording length in seconds")] private float recordLength = 2f;

    [Tooltip("Which recording is this")] private int recordingNum = 0;

    [Tooltip("For displaying saves being changed")] private SaveAssigner saveAssigner;

    [SerializeField, Tooltip("Button that begins recording")] private UnityEngine.UI.Button recordButton;

    // Awake is called when the script is being loaded
    private void Awake()
    {
        baseText = textField.text;
        SetText(0f);
    }

    // Update is called once per frame
    private void Update()
    {
        // If a recording is active, record
        if (recordAcceleration)
        {
            DoRecording();
        }

    }

    /// <summary>
    /// Records acceleration input and updates timer, saves acceleration when done recording
    /// </summary>
    private void DoRecording()
    {
        // Figure out how long this recording has gone on for
        float recordingMoment = Time.time - recordStart;

        //var acceleration = Accelerometer.current.acceleration.ReadValue();
        var acceleration = Input.acceleration;

        // Increase current filter acceleration and current filter level
        filterAcceleration += acceleration.z;
        currentFilter++;

        // If the filter level is bypassed
        if (currentFilter >= filterLevel)
        {
            // Record a filtered acceleration level and the current time of recording
            sumAcceleration.Add(new Vector2(filterAcceleration / currentFilter, recordingMoment));

            // Reset the filter acceleration and current filter level
            filterAcceleration *= 0;
            currentFilter *= 0;
        }

        // If the recording has been going on for more than the recordlength time, stop the recording
        if (recordingMoment >= recordLength)
        {
            SetText(0f);
            recordAcceleration = false;
            SaveRecording(sumAcceleration);
        }
        else
        {
            // Update the timer text to read the current recording time
            SetText(recordLength - recordingMoment);
        }
    }

    /// <summary>
    /// Saves a recording
    /// </summary>
    /// <param name="toBeSaved"></param>
    /// <param name="recordingNum"></param>
    private void SaveRecording(List<Vector2> toBeSaved)
    {
        string folderPath = Application.persistentDataPath + @"\AccelerationData\";

        string filePath = folderPath + string.Format("acceleration_recording{0}.csv", recordingNum);

        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        // Delete file if it already exists
        if (File.Exists(filePath)) File.Delete(filePath);



        // Create a file to write to.
        TextWriter tw = new StreamWriter(filePath, false);

        tw.WriteLine("Acc:;Time:");
        tw.Close();

        tw = new StreamWriter(filePath, true);
        foreach (Vector2 acc_time_pair in toBeSaved)
        {
            tw.WriteLine(acc_time_pair.x + ";" + acc_time_pair.y);
        }
        tw.Close();

        saveAssigner.AssignSaveSlot(recordingNum + 1);

        // Make button interactable again
        recordButton.interactable = true;
    }

    /// <summary>
    /// Updates the text in the textfield
    /// </summary>
    /// <param name="value"></param>
    private void SetText(float value)
    {
        textField.text = string.Format(baseText, value);
    }

    public void GetSaveAssigner(SaveAssigner newSaveAssigner)
    {
        saveAssigner = newSaveAssigner;
    }

    /// <summary>
    /// Change recording number
    /// </summary>
    /// <param name="newNum"></param>
    public void ChangeRecordingNumb(int newNum)
    {
        recordingNum = newNum;
    }

    /// <summary>
    /// Changes filterlevel
    /// </summary>
    /// <param name="newFilterLevel"></param>
    public void ChangeFilter(float newFilterLevel)
    {
        filterLevel = (int)newFilterLevel;
    }

    /// <summary>
    /// Changes recordLength
    /// </summary>
    /// <param name="newRecordLength"></param>
    public void ChangeRecordLength(float newRecordLength)
    {
        recordLength = newRecordLength;
    }

    public void BeginRecording()
    {
        // Reset the time
        recordStart = Time.time;
        // Reset the list
        sumAcceleration = new List<Vector2>();
        // Begin recording
        recordAcceleration = true;

        // Make button not interactable
        recordButton.interactable = false;
    }
}
