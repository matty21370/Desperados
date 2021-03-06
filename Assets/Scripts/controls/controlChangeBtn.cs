using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlChangeBtn : MonoBehaviour
{
    [SerializeField] private GameObject owner;
    private Text txt;
    void Start()
    {
        
    }

    private KeyCode refworkingKey;
    private KeyCode newKey;
    public void SetText(string text)
    {
        if (text.Equals("foward"))
		{
           

            buttonPress("Ftext");  
           owner.GetComponent<Player>().setControl(newKey, "foward");
        }
        if (text.Equals("backward"))
        {
           
            buttonPress("Btext");
            owner.GetComponent<Player>().setControl(newKey, "backward");
        }
        if (text.Equals("left"))
        {
            buttonPress("Ltext");
            owner.GetComponent<Player>().setControl(newKey, "left");
        }
        if (text.Equals("right"))
        {
            
            buttonPress("Rtext");
            owner.GetComponent<Player>().setControl(newKey, "right");
        }
        if (text.Equals("up"))
        {
           
            buttonPress("Utext");
            owner.GetComponent<Player>().setControl(newKey, "up");
        }
        if (text.Equals("down"))
        {
            
            buttonPress( "Dtext");
            owner.GetComponent<Player>().setControl(newKey, "down");

        }
        if (text.Equals("shoot"))
        {
            
            buttonPress( "Stext");
            owner.GetComponent<Player>().setControl(newKey, "shoot");
        }
     
        if (text.Equals("boost"))
        {

            buttonPress("BoostText");
            owner.GetComponent<Player>().setControl(newKey, "boost");

        }
       
        if (text.Equals("shop"))
        {

            buttonPress("Shoptext");
            owner.GetComponent<Player>().setControl(newKey, "shop");
        }
        if (text.Equals("score"))
        {

            buttonPress("Scoretext");
            owner.GetComponent<Player>().setControl(newKey, "score");
        }
        if (text.Equals("deployMine"))
        {

            buttonPress("DMinetext");
            owner.GetComponent<Player>().setControl(newKey, "mine");
        }
        if (text.Equals("find"))
        {

            buttonPress("FPtext");
            owner.GetComponent<Player>().setControl(newKey, "find");
        }
        if (text.Equals("flip"))
        {

            buttonPress("Fliptext");
            owner.GetComponent<Player>().setControl(newKey, "flip");
        }
    }

    private void buttonPress(string buttonText)
	{
        refworkingKey = owner.GetComponent<Player>().getKeySet();
        getKey();
        Debug.Log(newKey);
        setButton(newKey, buttonText);
       // owner.GetComponent<Player>().setControl(newKey, buttonText);
    }

    private void getKey()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (vKey != KeyCode.Return)
                {
                    newKey = vKey; //this saves the key being pressed               
                  //  bDetectKey = false;
                }
            }
        }
        
    }
    
    private void setButton(KeyCode code, string buttonSearch)
    {
        Text txt = transform.Find(buttonSearch).GetComponent<Text>();
        txt.text = newKey.ToString();
        
    }

}
