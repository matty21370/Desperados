using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour
{


    [SerializeField]
    private TMP_Text levelText;
    void Start()
    {
        if (PlayerPrefs.HasKey("Level")) //If the 'Level' key exists in the players registry
        {
            levelText.text = PlayerPrefs.GetInt("Level").ToString(); //We want to update the level text to reflect the key
        }
    }
}
