using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
	[SerializeField] private GameObject owner;
	
	private Text txt;
	private bool altern = false;
	public void SetText(string text)
	{
		if (text.Equals("health"))
		{
			int price = 100;
			if (owner.GetComponent<Player>().getCurrency() >= price)
			{

				Text txt = transform.Find("healthText").GetComponent<Text>();
				txt.text = "Purchased";
				GetComponent<Button>().interactable = false;

				owner.GetComponent<Player>().upgradePurchasedHealth();
				owner.GetComponent<Player>().UpdateHealthBar();
				//owner.GetComponent<Player>().purchaseMade(price);
				makePurchase(price);
			}
			else
			{
				altText("healthText", "Increase Health: \n 100 Points");
			}

		}else if (text.Equals("speed"))
		{
			int price = 50;
			if (owner.GetComponent<Player>().getCurrency() >= price)
			{

				Text txt = transform.Find("speedText").GetComponent<Text>();
				txt.text = "Purchased";
				GetComponent<Button>().interactable = false;

				owner.GetComponent<Player>().upgradePurchasedSpeed();
				
				makePurchase(price);
			}
			else
			{
				altText("speedText", "Increase Speed: \n 50 Points");
			}
				
		}
		else if (text.Equals("mine"))
		{
			int price = 20;
			if (owner.GetComponent<Player>().getCurrency() >= price)
			{
				Text txt = transform.Find("mineText").GetComponent<Text>();
				txt.text = "Purchased";
				GetComponent<Button>().interactable = false;

				owner.GetComponent<Player>().unlockMines();
				
				makePurchase(price);
			}
			else
			{
				altText("mineText", "Unlock Mines: \n 20 Points");
			}

		}
	}

	private void makePurchase(int price)
	{
		owner.GetComponent<Player>().purchaseMade(price);
		owner.GetComponent<Player>().getShop().updateText(owner.GetComponent<Player>().getCurrency());
	}

	private void altText(string buttonNeeded,string item)
	{
		if (!altern)
		{
			Text txt = transform.Find(buttonNeeded).GetComponent<Text>();
			txt.text = "Insufficient Funds";
			altern = true;
			Debug.Log(altern);


		}
		else if (altern)
		{
			Text txt = transform.Find(buttonNeeded).GetComponent<Text>();
			txt.text =  item;
			altern = false;
			Debug.Log(altern);
		}
	}
	
}
