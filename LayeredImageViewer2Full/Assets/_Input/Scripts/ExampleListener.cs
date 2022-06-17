using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class ExampleListener : MonoBehaviour
{
    public ButtonHandler primaryAxisClickHandler = null;
    public ButtonHandler secondaryAxisClickHandler = null;

    //Move Object (the floor in this case)
    public GameObject Ground;


    public void OnEnable()
    {
        primaryAxisClickHandler.OnButtonDown += PrintPrimaryButtonDown;
        //primaryAxisClickHandler.OnButtonUp += PrintPrimaryButtonUp;

        secondaryAxisClickHandler.OnButtonDown += PrintSecondaryButtonDown;
        //secondaryAxisClickHandler.OnButtonUp += PrintSecondaryButtonUp;
    }
    
    public void OnDisable()
    {
        primaryAxisClickHandler.OnButtonDown -= PrintPrimaryButtonDown;
        //primaryAxisClickHandler.OnButtonUp -= PrintPrimaryButtonUp;

        secondaryAxisClickHandler.OnButtonDown -= PrintSecondaryButtonDown;
        //secondaryAxisClickHandler.OnButtonUp -= PrintSecondaryButtonUp;
    }

    private void PrintPrimaryButtonDown(XRController controller)
    {
        //Debug
        print("Primary Button Down");

        //Move Down function:
        Ground = Instantiate(Ground, transform.position, transform.rotation);
        Ground.transform.position -= new Vector3(0, 0.1f, 0);
    }

    private void PrintSecondaryButtonDown(XRController controller)
    {
        //Debug
        print("Secondary Button Down");

        //Move Up function:
        Ground = Instantiate(Ground, transform.position, transform.rotation) as GameObject;
        Ground.transform.position += new Vector3(0, 0.1f, 0);
    }


    //Button Release Functions
    /*
    private void PrintPrimaryButtonUp(XRController controller)
    {
        print("Primary Button Up");
    }

    private void PrintSecondaryButtonUp(XRController controller)
    {
        print("Secondary Button Up");
    }
    */

    private void PrintPrimaryAxis(XRController controller, Vector2 value)
    {

    }

    private void PrintTrigger(XRController controller, float value)
    {

    }
}
