using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButton : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Springs")]
    public SpringJoint[] springs;
    public float d, k, minDistance;
    private Rigidbody _rb;
    AudioSource audioS;
    public float min=0.75f, max=1.25f;

    public Text keyText;
       
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        foreach (SpringJoint js in springs)
        {
            js.damper = d;
            js.spring = k;

            js.minDistance = minDistance;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
        localVelocity.x = 0;
        localVelocity.z = 0;

        _rb.velocity = transform.TransformDirection(localVelocity);

        //clamp  position
        float clampedY = Mathf.Clamp(transform.localPosition.y, min, max);

        transform.localPosition = new Vector3(0,clampedY,0);

    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("button"))
        {
            audioS.Play();

            //call actions
            Action();
        }
    }


    public void Action()
    {
        if(keyText.text=="go")
        {
            PhysicKeyboard.PKB.Accept();
        }
        else if (keyText.text == "del")
        {
            PhysicKeyboard.PKB.RemoveChar();
        }
        else if (keyText.text == "shift")
        {
            PhysicKeyboard.PKB.ChangeUperLower();
        }
        else
        {
            PhysicKeyboard.PKB.AddChar(keyText.text);
        }

    }



    
}
