using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
	private bool shopEnabled;
	private GameObject shop;

	private int allSlots;
	private int enabeldSlots;
	private GameObject[] slot;

	//public GameObject slotHolder;

	void Start()
	{
		allSlots = 4;
		slot = new GameObject[allSlots];
		shopEnabled = false;
	/*	for (int i = 0; i < allSlots; i++)
		{
			slot[i] = slotHolder.transform.GetChild(i).gameObject;
			if (slot[i].GetComponent<Slot>().item == null)
			{
				slot[i].GetComponent<Slot>().empty = true;
			}
		}*/
	}

	public void OpenShop()
	{


	}

	void Update( )
	{

		if (Input.GetKeyDown(KeyCode.I)) //If the user presses the T key on the keyboard
		{
			// GameObject.Find("shop").GetComponent<Shop>().setEnabled();
		setEnabled();
		}

		if (shopEnabled)
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
		}

	}
	public void setEnabled()
	{
		shopEnabled = !shopEnabled;
	}


	private void onTriggerEnter(Collider other)
	{
		if (other.tag == "Item")
		{
			GameObject itemToStore = other.gameObject;
			Item item = itemToStore.GetComponent<Item>();


			AddItem(itemToStore, item.ID, item.type, item.description, item.icon);

		}
	}

	void AddItem(GameObject itemObject, int itemId, string itemType, string description, Sprite itemIcon)
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
	}


}
