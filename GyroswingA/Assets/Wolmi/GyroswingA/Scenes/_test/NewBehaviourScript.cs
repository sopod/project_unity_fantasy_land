using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            this.transform.Translate(transform.forward * 10f * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.S))
            this.transform.Translate(-transform.forward * 10f * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            this.transform.Rotate(transform.up * 10f * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            this.transform.Rotate(-transform.up * 10f * Time.deltaTime);
    }
}
