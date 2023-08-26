using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScnMang : MonoBehaviour
{
    // Start is called before the first frame update
    public static ScnMang SM;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        if(InputManager.instance.Two_R_DW)
        {
            SceneManager.LoadScene(0);
            
        }
        
    }

    public void GoToScene(int a)
    {
        SceneManager.LoadScene(a);
    }
}
