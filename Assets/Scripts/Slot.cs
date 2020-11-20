using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
	public GameObject item;
	public bool empty;
	public Sprite icon;
	public string type;
	public int ID;
	public string description;



	public void UpdateSlot()
	{
		this.GetComponent<Image>().sprite = icon;
	}
}
