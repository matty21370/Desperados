using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsPage : MonoBehaviour
{
	public bool controlsEnabled;
	private GameObject controlsPage;
	//	[SerializeField] private GameManager gameManager;
	private GameManager gameManager;
	[SerializeField] private GameObject owner;



	public void setEnabled()
	{
		controlsEnabled = !controlsEnabled;
	}
}
