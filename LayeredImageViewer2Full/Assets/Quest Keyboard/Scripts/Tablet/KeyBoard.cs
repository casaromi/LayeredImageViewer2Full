using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour {

	public Text objectiveText;
	public Canvas mainCanvas;
	public bool conserveText=false;
    
    // this is the font of the texts
    public Font f;

	string previousText;
	float elapsed;
	bool capitalLeters,symbolMode;
	int nb_leters;
	public Text[] charText;
	public Text[] charSymbol;
	string actualTXT;

    public MyEvent acceptEvent;


    public void updateFont()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("but");

   
        nb_leters = go.Length;
        charText = new Text[nb_leters];
        charSymbol = new Text[nb_leters];

        for (int ii = 0; ii < nb_leters; ii++)
        {
            charText[ii] = go[ii].transform.GetChild(0).GetComponent<Text>();
            charSymbol[ii] = go[ii].transform.GetChild(1).GetComponent<Text>();

            charText[ii].font = f;
            charSymbol[ii].font = f;
        }
    }
	void Start () 
	{
        updateFont();
                
        elapsed =0;
		
		symbolMode=false;
		capitalLeters=false;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		// only blink when a text is selected
		elapsed+=Time.fixedDeltaTime;
		
		
	
	}


    //this function is called to write a char
	public void writeChar(Text txt)
	{
		objectiveText.text=actualTXT+txt.text;
		actualTXT=objectiveText.text;
	
	}


    //this function errases a char
	public void errase()
	{
		if(actualTXT.Length>0)
		{
			objectiveText.text=actualTXT.Remove(actualTXT.Length-1);
		}

		actualTXT=objectiveText.text;
	}


	

    // when we press ok
	public void acceptText()
	{
        acceptEvent.Invoke();

    }

    //when we press cancel
	public void cancelText()
	{
		
		objectiveText.text=previousText;
		mainCanvas.enabled=false;

		objectiveText=null;
	}

    //change leters to upper case
	public void uperLowerCase()
	{

		if(capitalLeters==false)
		{
			for(int ii=0; ii<nb_leters;ii++)
			{
				charText[ii].text=charText[ii].text.ToUpper();
				capitalLeters=true;
			}
		}
		else
		{
			for(int ii=0; ii<nb_leters;ii++)
			{
				charText[ii].text=charText[ii].text.ToLower();
				capitalLeters=false;
			}
		}
	}
	
    //change lettrers to symbols
	public void symbolChangeMode()
	{

		if(symbolMode==false)
		{
			
			string temp1;
			for(int ii=0; ii<nb_leters;ii++)
			{
				temp1=charText[ii].text;
				charText[ii].text=charSymbol[ii].text;
				charSymbol[ii].text=temp1;
			}

			symbolMode=true;



		}
		else
		{
			string temp2;
			for(int ii=0; ii<nb_leters;ii++)
			{
				temp2=charSymbol[ii].text;
				charSymbol[ii].text=charText[ii].text;
				charText[ii].text=temp2;
			}

			symbolMode=false;

		}
		
	}




}


