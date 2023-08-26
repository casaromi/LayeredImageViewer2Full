using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactButton : MonoBehaviour
{
    // Start is called before the first frame update
    float th = 0.20f;

    public float exitTime = 0;
    public float enterTime = 0;
    public float lastEnterTime = -1000;
    Color col;
    public bool triggered=false;

    void Start()
    {
        col = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        


    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "indexFinger")
        {
            lastEnterTime = enterTime;

            enterTime = Time.fixedTime;

            GetComponent<Image>().color = Color.red;

            triggered = false;
        }
    }

    /*public void OnTriggerExit(Collider other)
    {
    
        if (other.tag == "indexFinger")
        {
            exitTime = Time.fixedTime;

            if (exitTime - enterTime > th && enterTime-lastEnterTime>th)
            {
                GetComponent<Button>().onClick.Invoke();

                GetComponent<Image>().color = col;
               // Debug.Log(other.tag);
            }
        }
    }
    */

    public void OnTriggerExit(Collider other)
    {

        if (other.tag == "indexFinger")
        {           
           GetComponent<Image>().color = col;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "indexFinger")
        {
            exitTime = Time.fixedTime;

            if ( (exitTime - enterTime > th && enterTime - lastEnterTime > th) && triggered==false)
            {
                GetComponent<Button>().onClick.Invoke();
                GetComponent<Image>().color = col;
                // Debug.Log(other.tag);

                triggered = true;
            }
        }
    }

}
