using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    // Start is called before the first frame update
    [Header("The hand controllers")]
    public XRController rightController;
    public XRController leftController;

    [Header("The Controller Buttons")]
    public InputHelpers.Button gripRight_but;
    public InputHelpers.Button gripLeft_but;
    public InputHelpers.Button triggerRight_but;
    public InputHelpers.Button triggerLeft_but;
    public InputHelpers.Button oneRight_but;
    public InputHelpers.Button oneLeft_but;
    public InputHelpers.Button twoRight_but;
    public InputHelpers.Button twoLeft_but;

    [Header("The Animators attached to the hands")]
    public Animator animRhand;
    public Animator animLhand;

    [Header("The colliders for riggidbodies of hands")]
    public HandGrabbing grabScriptLeft;
    public HandGrabbing grabScriptRight;



    private bool gripLeft, gripRight;
    private bool triggerLeft, triggerRight;
    private bool oneLeft, oneRight;
    private bool twoLeft, twoRight;

    private bool gripLeft_prev, gripRight_prev;
    private bool triggerLeft_prev, triggerRight_prev;
    private bool oneLeft_prev, oneRight_prev;
    private bool twoLeft_prev, twoRight_prev;



    [Header("Public values of buttons")]
    public bool G_R;
    public bool G_R_DW;
    public bool G_R_UP;

    public bool G_L;
    public bool G_L_DW;
    public bool G_L_UP;

    public bool T_R;
    public bool T_R_DW;
    public bool T_R_UP;

    public bool T_L;
    public bool T_L_DW;
    public bool T_L_UP;

    public bool One_R_DW;
    public bool One_L_DW;

    public bool Two_R_DW;
    public bool Two_L_DW;


    [Header("Values of X and Y of the controllers")]
    public Vector2 axisR;
    public Vector2 axisL;



    void Awake()
    {
        instance =this;
     
    }
    private void Start()
    {
        grabScriptRight.EnableColliders(false);
        grabScriptLeft.EnableColliders(false);
    }

    // Update is called once per frame
    void Update()
    {

        //reset triggers
        ResetState();


        //button check RIGHT controller
        CheckGripRight();
        CheckTriggerRight();

        //used for hands animations
        HandAnimations();
        
        CheckButtonOneRight();
        CheckButtonTwoRight();

        //button check LEFT controller
        CheckGripLeft();
        CheckTriggerLeft();
        CheckButtonOneLeft();
        CheckButtonTwoLeft();

 
        //axis values
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisL);
        rightController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisR);


       

    }

    public void ResetState()
    {
        G_R_DW=false;
        G_R_UP = false;
        G_L_DW = false;
        G_L_UP = false;

        T_R_DW = false;
        T_R_UP = false;
        T_L_DW = false;
        T_L_UP = false;

        One_R_DW = false;
        One_L_DW = false;

        Two_R_DW = false;
        Two_L_DW = false;
    }


    #region RIGHT HAND

    public void CheckGripRight()
    {
        
        if(rightController.inputDevice.IsPressed(gripRight_but, out gripRight, rightController.axisToPressThreshold))
        {
            G_R = gripRight;

            if (gripRight != gripRight_prev)
            {

                if (gripRight)
                {
                    G_R_DW = true;
                    G_R_UP = false;
                    grabScriptRight.EnableColliders(true);
                    //animRhand.SetFloat("grab", 1);
                }
                else
                {
                    G_R_UP = true;
                    G_R_DW = false;
                    grabScriptRight.EnableColliders(false);
                    //animRhand.SetFloat("grab", 0);
                }

                gripRight_prev = gripRight;
            }
        }

        
    }

    
    public void CheckTriggerRight()
    {

        if (rightController.inputDevice.IsPressed(triggerRight_but, out triggerRight, rightController.axisToPressThreshold))
        {
            T_R = triggerRight;

            if (triggerRight != triggerRight_prev)
            {
                if (triggerRight)
                {
                    T_R_DW = true;
                    T_R_UP = false;

                    //animRhand.SetFloat("pick", 1);
                }
                else
                {
                    T_R_UP = true;
                    T_R_DW = false;

                    //animRhand.SetFloat("pick", 0);
                }

                triggerRight_prev = triggerRight;
            }
        }


    }




    public void CheckButtonOneRight()
    {
  
        if (rightController.inputDevice.IsPressed(oneRight_but, out oneRight, rightController.axisToPressThreshold))
        {

            if (oneRight != oneRight_prev)
            {
                if (oneRight)
                {
                    One_R_DW = true;

                }
                else
                {
                    One_R_DW = false;
                }

                oneRight_prev = oneRight;
            }
        }


    }

    public void CheckButtonTwoRight()
    {

        if (rightController.inputDevice.IsPressed(twoRight_but, out twoRight, rightController.axisToPressThreshold))
        {
            if (twoRight != twoRight_prev)
            {
                if (twoRight)
                {
                    Two_R_DW = true;
                }
                else
                {
                    Two_R_DW = false;
                }

                twoRight_prev = twoRight;
            }
        }

       

    }

    #endregion

    #region LEFT HAND

    public void CheckGripLeft()
    {

        if (leftController.inputDevice.IsPressed(gripLeft_but, out gripLeft, leftController.axisToPressThreshold))
        {
            G_L = gripLeft;

            if (gripLeft != gripLeft_prev)
            {

                if (gripLeft)
                {
                    G_L_DW = true;
                    G_L_UP = false;

                    grabScriptLeft.EnableColliders(true);
                    //animLhand.SetFloat("grab", 1);
                }
                else
                {
                    G_L_UP = true;
                    G_L_DW = false;
                    grabScriptLeft.EnableColliders(false);
                    //animLhand.SetFloat("grab", 0);
                }

                gripLeft_prev = gripLeft;
            }
        }


    }


    public void CheckTriggerLeft()
    {

        if (leftController.inputDevice.IsPressed(triggerLeft_but, out triggerLeft, leftController.axisToPressThreshold))
        {

            T_L = triggerLeft;
            if (triggerLeft != triggerLeft_prev)
            {

                if (triggerLeft)
                {
                    T_L_DW = true;
                    T_L_UP = false;

                    //animLhand.SetFloat("pick", 1);
                }
                else
                {
                    T_L_UP = true;
                    T_L_DW = false;
                    //animLhand.SetFloat("pick", 0);
                }

                triggerLeft_prev = triggerLeft;
            }
        }


    }



    public void CheckButtonOneLeft()
    {

        if (leftController.inputDevice.IsPressed(oneLeft_but, out oneLeft, leftController.axisToPressThreshold))
        {
            if (oneLeft != oneLeft_prev)
            {

                if (oneLeft)
                {
                    One_L_DW = true;
                }
                else
                {
                    One_L_DW = false;
                }

                oneLeft_prev = oneLeft;
            }
        }


    }

    public void CheckButtonTwoLeft()
    {

        if (leftController.inputDevice.IsPressed(twoLeft_but, out twoLeft, leftController.axisToPressThreshold))
        {

            if (twoLeft != twoLeft_prev)
            {

                if (twoLeft)
                {
                    Two_L_DW = true;
                }
                else
                {
                    Two_L_DW = false;
                }

                twoLeft_prev = twoLeft;
            }
        }



    }


    #endregion


    #region HandAnim

    void HandAnimations()
    {
        //right controller
        float grab_value;
        rightController.inputDevice.TryGetFeatureValue(CommonUsages.grip, out grab_value);
        float trigger_value;
        rightController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out trigger_value);

        animRhand.SetFloat("grab", grab_value);
        animRhand.SetFloat("pick", trigger_value);


        //left controller
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.grip, out grab_value);
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out trigger_value);

        animLhand.SetFloat("grab", grab_value);
        animLhand.SetFloat("pick", trigger_value);
    }

    #endregion


}
