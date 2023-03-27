using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPrefs : MonoBehaviour
{
    /// <summary>
    /// Deletes all PlayerPrefs
    /// </summary>
    public void ResetAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
