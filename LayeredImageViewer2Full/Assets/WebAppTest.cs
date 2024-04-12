using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WebAppTest : MonoBehaviour
{
   
    public GameObject webAppRunner;

    // Array of sprites for input from editor
    public Sprite [] sprites;

    

    // Start is called before the first frame update
    void Start()
    {
        // initial test, just package up some simple JSON and send it
        // to an app that parses it, multiplies it by 2, jsonifies the result
        // and sends it back
        webAppRunner.GetComponent<WebAppRunner>().StartTest1(2.0);

        webAppRunner.GetComponent<WebAppRunner>().StartTest2(sprites);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
