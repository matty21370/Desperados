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
           refworkingKey= owner.GetComponent<Player>().getKeySet();
            getKey();
            Debug.Log(newKey);

            setButton(newKey, "Ftext");
            owner.GetComponent<Player>().setControl(newKey,text);
        }
        if (text.Equals("backward"))
        {
            refworkingKey = owner.GetComponent<Player>().getKeySet();
            getKey();
            Debug.Log(newKey);

            setButton(newKey, "Btext");
            owner.GetComponent<Player>().setControl(newKey, text);
        }
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
