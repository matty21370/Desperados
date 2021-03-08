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
        else if (text.Equals("backward"))
        {
           
            buttonPress("Btext");
            owner.GetComponent<Player>().setControl(newKey, "backward");
        }
        else if (text.Equals("left"))
        {
            buttonPress("Ltext");
            owner.GetComponent<Player>().setControl(newKey, "left");
        }
        else if (text.Equals("right"))
        {
            
            buttonPress("Rtext");
            owner.GetComponent<Player>().setControl(newKey, "right");
        }
       else if (text.Equals("flip"))
        {

            buttonPress("Fliptext");
            owner.GetComponent<Player>().setControl(newKey, "Flip");
        }
        else if (text.Equals("up"))
        {
           
            buttonPress("Utext");
            owner.GetComponent<Player>().setControl(newKey, "up");
        }
        else if (text.Equals("down"))
        {
            
            buttonPress( "Dtext");
            owner.GetComponent<Player>().setControl(newKey, "down");

        }
        else if (text.Equals("shoot"))
        {
            
            buttonPress( "Stext");
            owner.GetComponent<Player>().setControl(newKey, "shoot");
        }
     
        else if (text.Equals("boost"))
        {

            buttonPress("BoostText");
            owner.GetComponent<Player>().setControl(newKey, "boost");

        }
       
        else if (text.Equals("shop"))
        {

            buttonPress("Shoptext");
            owner.GetComponent<Player>().setControl(newKey, "shop");
        }
        else if (text.Equals("score"))
        {

            buttonPress("Scoretext");
            owner.GetComponent<Player>().setControl(newKey, "score");
        }
        else if (text.Equals("deployMine"))
        {

            buttonPress("DMinetext");
            owner.GetComponent<Player>().setControl(newKey, "mine");
        }
        else if (text.Equals("find"))
        {

            buttonPress("FPtext");
            owner.GetComponent<Player>().setControl(newKey, "find");
        }
        else if (text.Equals("flip"))
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
