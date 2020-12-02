using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpNotification : MonoBehaviour
{
    public Text levelText;

    public void SetText(string text)
    {
        levelText.text = text;
    }
}
