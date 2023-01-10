using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public float flip;
    public GameObject side1;
    public GameObject side2; 

    // Start is called before the first frame update
    void Start()
    {
        side1 = GameObject.Find("Flip");
        side2 = GameObject.Find("Flipped");
        side2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("i"))
        {
            transform.Rotate(new Vector3(0, flip, 0) * Time.deltaTime);


            side1.SetActive(false);
            side2.SetActive(true);
        }





        
    }
}
