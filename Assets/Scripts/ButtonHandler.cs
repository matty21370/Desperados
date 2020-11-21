using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
	[SerializeField] private GameObject owner;
	public void SetText(string text)
	{
		Text txt = transform.Find("healthText").GetComponent<Text>();
		txt.text = text;
		GetComponent<Button>().interactable = false;
		owner.GetComponent<Player>().upgradePurchasedHealth();
		owner.GetComponent<Player>().UpdateHealthBar();
	}
}
