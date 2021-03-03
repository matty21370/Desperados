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
            else
            {
                altText("TrailOneTxt", "Purple Trail \n Unlocks at Level 10");
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
            else
            {
                altText("TrailTwoTxt", "Blue Trail \n Unlocks at Level 15");
            }
        }
        else if (text.Equals("level20"))
        {
            if (checkLevel(20))
            {
                PlayerPrefs.SetInt("custom", 20);
                PlayerPrefs.Save();
                Debug.Log("SET");
            }
            else
            {
                altText("TrailThreeTxt", "Green Trail \n Unlocks at Level 20");
            }
        }
        else if(text.Equals("trailRest"))
        {
            PlayerPrefs.SetInt("custom", 0);
            PlayerPrefs.Save();
            Debug.Log("SET");
        }
        else if (text.Equals("BulletRest"))
        {
            PlayerPrefs.SetInt("bulletCustom", 0);
            PlayerPrefs.Save();
            Debug.Log("SET");
        }
        else if (text.Equals("BulletOne"))
        {
            
            if (checkLevel(10))
            {
                PlayerPrefs.SetInt("bulletCustom", 1);
                PlayerPrefs.Save();
                Debug.Log("SET");
            }
            else
            {
                altText("BulletOneTxt", "Purple Bullet \n Unlocks at Level 10");
            }

        }
        else if (text.Equals("BulletTwo"))
        {

            if (checkLevel(15))
            {
                PlayerPrefs.SetInt("bulletCustom", 2);
                PlayerPrefs.Save();
                Debug.Log("SET");
            }
            else
            {
                altText("BulletTwoTxt", "Purple Bullet \n Unlocks at Level 15");
            }

        }
        else if (text.Equals("BulletThree"))
        {

            if (checkLevel(20))
            {
                PlayerPrefs.SetInt("bulletCustom", 3);
                PlayerPrefs.Save();
                Debug.Log("SET");
			}
			else
			{
                altText("BulletThreeTxt", "Aqua Bullet \n Unlocks at Level 20");
            }

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



    /// <summary>
	/// used when the player cannot afford the item 
	/// <param name="buttonNeeded"> 
	/// the name of the button in string format
	/// <paramref name="item"/>
	/// the details of the item
	/// </param>
	/// </summary>
	[PunRPC]
    private void altText(string buttonNeeded, string item)
    {

        Text txt = transform.Find(buttonNeeded).GetComponent<Text>();
        txt.text = "Not High Enough \n Level";


        StartCoroutine(CoolDownTimer(buttonNeeded, item));
        GetComponent<Button>().interactable = true;
    }
    private void resetText(string buttonNeeded, string item)
    {
        Debug.Log("after");
        Text txt = transform.Find(buttonNeeded).GetComponent<Text>();
        txt.text = item;
        ///	altern = false;
      //  Debug.Log(altern);
    }


    [PunRPC]
    private IEnumerator CoolDownTimer(string buttonNeeded, string item)
    {
        yield return new WaitForSeconds(2f);
        resetText(buttonNeeded, item);

    }
}
