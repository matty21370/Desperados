using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
	[SerializeField] private GameObject owner;
	
	private Text txt;
	private bool altern = false;

	///<summary>
	///Runs when the button is clicked 
	///<param name="text">
	///The text given from the putton to identify it
	/// </param>
	///</summary>
	public void SetText(string text)
	{
		//the health button 
		if (text.Equals("health"))
		{
		
			int price = 100;
			//set price of the item
			if (owner.GetComponent<Player>().getCurrency() >= price)
			{
				//if enough points
				dissableButton("healthText");
				//dissable the button and set text to purchased

				owner.GetComponent<Player>().upgradePurchasedHealth();
				//run players upgrade
				
				makePurchase(price);
				//deal with the cost
			}
			else
			{
				altText("healthText", "Increase Health: \n 100 Points");
				//run alternate if not enough points 
			}

		}else if (text.Equals("speed"))
			//speed button see health for more details
		{
			int price = 50;
			if (owner.GetComponent<Player>().getCurrency() >= price)
			{

				dissableButton("speedText");

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
			//mine button see health for more details
			int price = 20;
			if (owner.GetComponent<Player>().getCurrency() >= price)
			{
				dissableButton("mineText");
				
				owner.GetComponent<Player>().unlockMines();
				
				makePurchase(price);
			}
			else
			{
				altText("mineText", "Unlock Mines: \n 20 Points");
			}

		}
		else if ((text.Equals("cooldown")))
		{
			//cooldown button see health for more details
			int price = 250;
			if (owner.GetComponent<Player>().getCurrency() >= price)
			{
				dissableButton("CoolText");
				
				owner.GetComponent<Player>().cooldownUpgrade();

				makePurchase(price);
			}
			else
			{
				altText("CoolText", "Decrease Weapon Cooldown: \n 250 Points");
			}
		}
	}


	/// <summary>
	/// take the cost of the item from the player and update the text
	/// <param name="price"> 
	/// the cost of the upgrade to be taken from the player
	/// </param>
	/// </summary>
	private void makePurchase(int price)
	{
		owner.GetComponent<Player>().purchaseMade(price);
		owner.GetComponent<Player>().getShop().updateText(owner.GetComponent<Player>().getCurrency());
	}




	/// <summary>
	/// dissable the item button
	/// <param name="buttonSearch"> 
	/// the name of the button in string format
	/// </param>
	/// </summary>
	private void dissableButton(string buttonSearch)
	{
		Text txt = transform.Find(buttonSearch).GetComponent<Text>();
		txt.text = "Purchased";
		GetComponent<Button>().interactable = false;
	}


	/// <summary>
	/// used when the player cannot afford the item 
	/// <param name="buttonNeeded"> 
	/// the name of the button in string format
	/// <paramref name="item"/>
	/// the details of the item
	/// </param>
	/// </summary>
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
