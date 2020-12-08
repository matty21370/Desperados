using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script created by: Matthew Burke
/// </summary>
public class Stats : MonoBehaviour
{
    /// <summary>
    /// This is a reference to the kills text in the stats window
    /// </summary>
    [SerializeField]
    private TMP_Text killsText;

    /// <summary>
    /// This is a reference to the deaths text in the stats window
    /// </summary>
    [SerializeField]
    private TMP_Text deathsText;

    /// <summary>
    /// This is a reference to the level text in the stats window
    /// </summary>
    [SerializeField]
    private TMP_Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Kills")) //If the 'Kills' key exists in the players registry
        {
            killsText.text = PlayerPrefs.GetInt("Kills").ToString(); //We want to update the kills text to reflect the key
        }
        if (PlayerPrefs.HasKey("Deaths")) //If the 'Deaths' key exists in the players registry
        {
            deathsText.text = PlayerPrefs.GetInt("Deaths").ToString(); //We want to update the deaths text to reflect the key
        }
        if (PlayerPrefs.HasKey("Level")) //If the 'Level' key exists in the players registry
        {
            levelText.text = PlayerPrefs.GetInt("Level").ToString(); //We want to update the level text to reflect the key
        }
    }
}
