using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorller : MonoBehaviour
{

    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        InputKeyboard();
    }
    void InputKeyboard()
    {

        if (Input.GetKey(KeyCode.W))
        {
            // transform.rotation = Quaternion.LookRotation(Vector3.forward);
            transform.position += Vector3.up * Time.deltaTime * 10;


        }
        else if (Input.GetKey(KeyCode.S))
        {
            // transform.rotation = Quaternion.LookRotation(Vector3.down);

            transform.position += Vector3.down * Time.deltaTime * 10;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // transform.rotation = Quaternion.LookRotation(Vector3.fo);
            transform.position += Vector3.left * Time.deltaTime * 10;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // transform.rotation = Quaternion.LookRotation(Vector3.forward);
            transform.position += Vector3.right * Time.deltaTime * 10;
        }
    }
}
