using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	public bool shopEnabled;
	private GameObject shop;

	private int allSlots=4;
	private int enabeldSlots;
	[SerializeField] private GameObject[] slots;
	private Text txt;
	//public GameObject slotHolder;

	void Start()
	{
	
	}

	public void updateTxt()
	{
		slots = new GameObject[allSlots+1];
		for (int i =1; i < allSlots+1; i++)
		{
		//	slots[i] = transform.GetChild(i).gameObject;
			//Debug.Log(slots[i]);
		//	Debug.Log(slots[i].transform.GetChild(i).gameObject);

		}
		

	}



		


	
	public void setEnabled()
	{
		shopEnabled = !shopEnabled;
	}

	public void updateText(int currency)
	{
		txt = transform.Find("Currency").GetComponent<Text>();
		txt.text = "Current Points:" + currency;
		
	}

	private void onTriggerEnter(Collider other)
	{
		if (other.tag == "Item")
		{
			GameObject itemToStore = other.gameObject;
			Item item = itemToStore.GetComponent<Item>();


			//AddItem(itemToStore, item.ID, item.type, item.description, item.icon);

		}
	}

	/*void AddItem(GameObject itemObject, int itemId, string itemType, string description, Sprite itemIcon)
	{
		for (int i = 0; i < allSlots; i++)
		{
			if (slot[i].GetComponent<Slot>().empty)
			{
				//add item
				itemObject.GetComponent<Item>().pickedUp = true;

				slot[i].GetComponent<Slot>().item = itemObject;
				slot[i].GetComponent<Slot>().icon = itemIcon;
				slot[i].GetComponent<Slot>().type = itemType;
				slot[i].GetComponent<Slot>().ID = itemId;
				slot[i].GetComponent<Slot>().description = description;


				itemObject.transform.parent = slot[i].transform;
				itemObject.SetActive(false);
				slot[i].GetComponent<Slot>().UpdateSlot();
				slot[i].GetComponent<Slot>().empty = false;
			}
			return;
		}
	}*/


}
