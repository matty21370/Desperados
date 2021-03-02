using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class MenuShopItem : MonoBehaviour
{
    public string name;
    public int price;
    public Sprite icon;
    public string description;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text customText;
    [SerializeField] private GameObject owner;
    private Text txt;








    public void SetText(string text)
    {
        if (text.Equals("level10"))
        {
            if (checkLevel(10))
            {
                PlayerPrefs.SetInt("custom", 10);
                PlayerPrefs.Save();
                Debug.Log("SET");
            }
        }
        else if (text.Equals("level15"))
        {
            if (checkLevel(15))
            {
                PlayerPrefs.SetInt("custom", 15);
                PlayerPrefs.Save();
                Debug.Log("SET");
            }
        }
        else
        {
            PlayerPrefs.SetInt("custom", 0);
            PlayerPrefs.Save();
            Debug.Log("SET");
        }

    }


    private bool checkLevel(int min)
    {
        int num = int.Parse(levelText.text);
        if (min <= num)
        {
            Debug.Log(num);
            return true;
        }
        else
        {
            Debug.Log("false");
            Debug.Log(num);
            return false;
        }

    }
}
