using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveButton : MonoBehaviour
{

	public bool leaveEnabled;
	private GameObject leaveButton;
	//	[SerializeField] private GameManager gameManager;
	private GameManager gameManager;
	[SerializeField] private GameObject owner;



	public void setEnabled()
	{
		leaveEnabled = !leaveEnabled;
	}

	public void SetText(string text)
	{
		//the health button 
		if (text.Equals("leave"))
		{
			//Debug.Log("leave game");
		//	leaveGame(owner);

		}
	}

	private void leaveGame(GameObject owner)
	{
		gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
		Application.Quit();
		
		//gameManager.LeaveRoom();
		//PhotonNetwork.LeaveRoom();
		owner.GetComponent<Player>().Disconnect();

	}

//	IEnumerator leaveGameSecond(GameObject owner)
	//{
		//gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
		//gameManager.Disconnect();
		/*while (PhotonNetwork.IsConnected)
		{
			yield return null;
		}
		SceneManager.LoadScene("Lobby");*/
	//}
}
