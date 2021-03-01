using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenuShopItem : MonoBehaviour
{
    public string name;
    public int price;
    public Sprite icon;
    public string description;
    [SerializeField]private TMP_Text levelText;
    [SerializeField] private TMP_Text customText;
    [SerializeField] private GameObject owner;
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
		else
		{
            PlayerPrefs.SetInt("custom", 0);
            PlayerPrefs.Save();
            Debug.Log("SET");
        }

    }


    private bool checkLevel(int num)
	{
        int min = int.Parse(levelText.text);
        if (min <= num)
		{
            return true;
		}
		else
		{
            return false;
		}

    }
    }
