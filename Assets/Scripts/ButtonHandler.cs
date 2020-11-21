using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
	[SerializeField] private GameObject owner;
	private int price=50;
	private Text txt;
	private bool altern = false;

	public void SetText(string text)
	{
		
		if (owner.GetComponent<Player>().getCurrency() >= price)
		{

			Text txt = transform.Find("healthText").GetComponent<Text>();
			txt.text = "Purchased";
			GetComponent<Button>().interactable = false;
			
			owner.GetComponent<Player>().upgradePurchasedHealth();
			owner.GetComponent<Player>().UpdateHealthBar();
		
		}
		else if (!altern)
		{
			Text txt = transform.Find("healthText").GetComponent<Text>();
			txt.text = "Insufficient Funds";
			altern = true;
			Debug.Log(altern);


		}
		else if (altern)
		{
			Text txt = transform.Find("healthText").GetComponent<Text>();
			txt.text = "Increase Health";
			altern = false;
			Debug.Log(altern);
		}
	
	}
	
}
