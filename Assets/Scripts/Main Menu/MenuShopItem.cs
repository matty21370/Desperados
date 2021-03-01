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

    public void SetText(string text)
    {
		if (text.Equals("level10"))
		{
			if (checkLevel(10))
			{
                Debug.Log("10");
			}
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
