using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MultipleOVRraycaster : MonoBehaviour
{

    [Header("The origin for the raycast")]
    public Transform raycastOrigin_L;
    public Transform raycastOrigin_R;

    [Header("The event systems")]
    public EventSystem eventS_L;
    public EventSystem eventS_R;


    /*
    [Header("Where the raycaster results appear")]
    public Text debugText_L;
    public Text debugText_R;
    */

        
    // this allows to know which gameobject is selected by the user
    [Header("The Selected gameobjects in the raycasters")]
    public GameObject selectedGameObject_L;
    public GameObject selectedGameObject_R;
    GameObject lastSelectedGameObject_L;
    GameObject lastSelectedGameObject_R;


    //pointer event data for both controllers
    private PointerEventData pointerData_L;
    private PointerEventData pointerData_R;
    private bool _guiRaycastHit_L;
    private bool _guiRaycastHit_R;


    [Header("The cursors used in the raycast")]
    public Transform cursor_L;
    public Transform cursor_R;

    [Header("The cursors used in the raycast")]
    public Color overColor;
    public Color normalColor;
    

    void Start()
    {

        //create pointer event data for left controller
        pointerData_L = new PointerEventData(eventS_L);
        //create pointer event data for left controller
        pointerData_R = new PointerEventData(eventS_R);
    }


    private void FixedUpdate()
    {

      

        //pointer exit left
        if (lastSelectedGameObject_L!=selectedGameObject_L)
        {
            //if the gameobject has a button
            if (lastSelectedGameObject_L != null)
            {
                if (lastSelectedGameObject_L.GetComponent<Image>() != null)
                {
                    lastSelectedGameObject_L.GetComponent<Image>().color = normalColor;
                }
            }
        }


        //pointer enter left
        if (selectedGameObject_L != null)
        {
            //if the gameobject has a button
            if (selectedGameObject_L.GetComponent<Image>() != null)
            {
                selectedGameObject_L.GetComponent<Image>().color = overColor;
            }
        }

        



        //pointer exit right
        if (lastSelectedGameObject_R != selectedGameObject_R)
        {
            //if the gameobject has a button
            if (lastSelectedGameObject_R != null)
            {
                if (lastSelectedGameObject_R.GetComponent<Image>() != null)
                {
                    lastSelectedGameObject_R.GetComponent<Image>().color = normalColor;
                }
            }
        }


        //pointer enter right
        if (selectedGameObject_R != null)
        {
            //if the gameobject has a button
            if (selectedGameObject_R.GetComponent<Image>() != null)
            {
                selectedGameObject_R.GetComponent<Image>().color = overColor;
            }
        }




        ///////////////////////
        /// LEFT CONTROLLER
        //////////////////////

        cursor_L.GetComponent<Renderer>().enabled = false;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(raycastOrigin_L.position, raycastOrigin_L.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "raycasterObjective")
            {
                pointerData_L.position = Camera.main.WorldToScreenPoint(hit.point);


                cursor_L.GetComponent<LineRenderer>().enabled = true;

                //set the points of the linerenderer
                cursor_L.GetComponent<LineRenderer>().SetPosition(0, raycastOrigin_L.position);
                cursor_L.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                cursor_L.GetComponent<Renderer>().enabled = true;

                cursor_L.position = hit.point;
                cursor_L.forward = hit.normal;


                //Debug.Log("Hit:" + hit.collider.gameObject.tag);
            }
            else
            {
                cursor_L.GetComponent<LineRenderer>().enabled = false;
                
            }

        }
        else
        {
            cursor_L.GetComponent<LineRenderer>().enabled = false;
            
        }





        ///////////////////////
        /// RIGHT CONTROLLER
        //////////////////////


        cursor_R.GetComponent<Renderer>().enabled = false;

        RaycastHit hit2;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(raycastOrigin_R.position, raycastOrigin_R.forward, out hit2, Mathf.Infinity))
        {
            if (hit2.collider.gameObject.tag == "raycasterObjective")
            {
                pointerData_R.position = Camera.main.WorldToScreenPoint(hit2.point);


                cursor_R.GetComponent<LineRenderer>().enabled = true;

                //set the points of the linerenderer
                cursor_R.GetComponent<LineRenderer>().SetPosition(0, raycastOrigin_R.position);
                cursor_R.GetComponent<LineRenderer>().SetPosition(1, hit2.point);

                cursor_R.GetComponent<Renderer>().enabled = true;

                cursor_R.position = hit2.point;
                cursor_R.forward = hit2.normal;


                //Debug.Log("Hit:" + hit.collider.gameObject.tag);
            }
            else
            {
                cursor_R.GetComponent<LineRenderer>().enabled = false;

            }

        }
        else
        {
            cursor_R.GetComponent<LineRenderer>().enabled = false;
         
        }






        /////////////////////
        /// dsiplay results
        /////////////////////

        /*
        if(debugText_L!=null && selectedGameObject_L!=null)
        {
            debugText_L.text = "" + selectedGameObject_L.name;
        }
        else if(debugText_L != null && selectedGameObject_L == null)
        {
            debugText_L.text = "null";
        }

        if (debugText_R != null && selectedGameObject_R!=null)
        {
            debugText_R.text = "" + selectedGameObject_R.name;
        }
        else if (debugText_R != null && selectedGameObject_R == null)
        {
            debugText_R.text = "null";
        }
        */


        lastSelectedGameObject_L =selectedGameObject_L;
        lastSelectedGameObject_R =selectedGameObject_R;


    }


    public void Update()
    {




        ///////////////
        /// left raycast
        /////////////////
        // results for the raycast
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData_L, results);

        if (results.Count > 0)
        {
            //WorldUI is my layer name
            if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                if (results[0].gameObject.GetComponent<Button>() != null)
                {
                    selectedGameObject_L = results[0].gameObject;
                }
                else if (results[0].gameObject.GetComponent<Text>() != null)
                {
                    selectedGameObject_L = results[0].gameObject.transform.parent.gameObject;
                }
                else
                {
                    selectedGameObject_L = null;
                }

                pointerData_L.eligibleForClick = true;
                pointerData_L.delta = Vector2.zero;
                pointerData_L.dragging = false;
                pointerData_L.useDragThreshold = false;
                pointerData_L.pressPosition = pointerData_L.position;
                pointerData_L.pointerPressRaycast = pointerData_L.pointerCurrentRaycast;

                results.Clear();
            }
            else
            {
                selectedGameObject_L = null;
            }
        }
        else
        {
            selectedGameObject_L = null;
        }


        ////////////////////
        /// right raycast
        ///////////////////
        List<RaycastResult> results2 = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData_R, results2);

        if (results2.Count > 0)
        {
            //WorldUI is my layer name
            if (results2[0].gameObject.layer == LayerMask.NameToLayer("UI"))
            {

                if (results2[0].gameObject.GetComponent<Button>() != null)
                {
                    selectedGameObject_R = results2[0].gameObject;
                }
                else if (results2[0].gameObject.GetComponent<Text>() != null)
                {
                    selectedGameObject_R = results2[0].gameObject.transform.parent.gameObject;
                }
                else
                {
                    selectedGameObject_R = null;
                }


                pointerData_R.eligibleForClick = true;
                pointerData_R.delta = Vector2.zero;
                pointerData_R.dragging = false;
                pointerData_R.useDragThreshold = false;
                pointerData_R.pressPosition = pointerData_R.position;
                pointerData_R.pointerPressRaycast = pointerData_R.pointerCurrentRaycast;

                results2.Clear();
            }
            else
            {
                selectedGameObject_R = null;
            }
        }
        else
        {
            selectedGameObject_R = null;
        }



        //get the triggers
        bool condition_L = InputManager.instance.T_L_DW;
        bool condition_R = InputManager.instance.T_R_DW;



        //in function of the conditions do this
        if (condition_L)
        {

            //if the raycast has selected a gameobject perform this actions
            if (selectedGameObject_L != null)
            {
                //if the gameobject has a button
                if (selectedGameObject_L.GetComponent<Button>() != null)
                {
                    selectedGameObject_L.GetComponent<Button>().onClick.Invoke();
                }
                else
                {
                    ExecuteEvents.Execute(selectedGameObject_L.gameObject, pointerData_L, ExecuteEvents.pointerClickHandler);
                }
            }
        }


        //in function of the conditions do this
        if (condition_R)
        {

            //if the raycast has selected a gameobject perform this actions
            if (selectedGameObject_R != null)
            {
                //if the gameobject has a button
                if (selectedGameObject_R.GetComponent<Button>() != null)
                {
                    selectedGameObject_R.GetComponent<Button>().onClick.Invoke();
                }
                else
                {
                    ExecuteEvents.Execute(selectedGameObject_R.gameObject, pointerData_R, ExecuteEvents.pointerClickHandler);

                }
            }
        }




    }
}





       
        


