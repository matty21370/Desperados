using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ButtonHandler : MonoBehaviour
{
	[SerializeField] private GameObject owner;

	private Text txt;
	private bool altern = false;
	private bool healthTextp = false;
	private bool mineTextp = false;
	private bool speedTextp = false;
	private bool coolTextp = false;
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
				healthTextp = true;
				makePurchase(price);
				//deal with the cost
			}
			else
			{
				altText("healthText", "Increase Health: \n 100 Points");
				//run alternate if not enough points 
			}

		} else if (text.Equals("speed"))
		//speed button see health for more details
		{
			int price = 50;
			if (owner.GetComponent<Player>().getCurrency() >= price) //remove for testing
			{//remove for testing

			dissableButton("speedText");//remove for testing

			owner.GetComponent<Player>().upgradePurchasedSpeed();//remove for testing
				speedTextp = true;
					makePurchase(price); //remove for testing
				}//remove for testing
				else //remove for testing
			{//remove for testing
				altText("speedText", "Increase Speed: \n 50 Points");//remove for testing
				}//remove for testing

		}
		else if (text.Equals("mine"))
		{
			//mine button see health for more details
			int price = 20;
			if (owner.GetComponent<Player>().getCurrency() >= price)
			{
				dissableButton("mineText");

				owner.GetComponent<Player>().unlockMines();
				mineTextp = true;
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
				coolTextp = true;
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


	public void Reset()
	{
		if (speedTextp)
		{
			altText("speedText", "Increase Speed: \n 50 Points");//remove for testing
			speedTextp = false;
		}
		if (coolTextp)
		{
			altText("CoolText", "Decrease Weapon Cooldown: \n 250 Points");
			coolTextp = false;
		}
		if (mineTextp)
		{
			altText("mineText", "Unlock Mines: \n 20 Points");
			mineTextp = false;
		}
		if (healthTextp)
		{

			altText("healthText", "Increase Health: \n 100 Points");
			healthTextp = false;
		}
 }
	/*void Update()
	{
		if (owner.GetComponent<Player>().getResetPlayer())
		{
			Reset();
			Debug.Log("RESET CALLED IN SHOP");
			owner.GetComponent<Player>().SetResetPlayer();
		}
	}
	*/
		private void enableButton(string buttonNeeded, string item)
	{
		Text txt = transform.Find(buttonNeeded).GetComponent<Text>();
		GetComponent<Button>().interactable = true;
		txt.text = item;
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
	[PunRPC]
	private void altText(string buttonNeeded,string item)
	{
	
			Text txt = transform.Find(buttonNeeded).GetComponent<Text>();
			txt.text = "More Funds Needed";
			
			
		StartCoroutine(CoolDownTimer( buttonNeeded, item));
		GetComponent<Button>().interactable = true;
	}
	private void resetText(string buttonNeeded, string item)
	{
		Debug.Log("after");
		Text txt = transform.Find(buttonNeeded).GetComponent<Text>();
		txt.text = item;
		///	altern = false;
		Debug.Log(altern);
	}


	[PunRPC]
	private IEnumerator CoolDownTimer(string buttonNeeded, string item)
	{
		yield return new WaitForSeconds(2f);
		resetText(buttonNeeded, item);

	}


	/***
	 * used to facilitate testing
	 * 
	 */
	public bool getSpeedTextp()
	{

		return speedTextp;
	}
}
