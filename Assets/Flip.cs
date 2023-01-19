using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public float flip;
    public GameObject side1;
    public GameObject side2;
    public float rotate;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        side1 = GameObject.Find("Flip");
        side2 = GameObject.Find("Flipped");
        Player = GameObject.Find("player");
        side2.SetActive(false);
        rotate = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y;
        Debug.Log(rotate);
      
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, flip, 0) * Time.deltaTime);
        if (Input.GetKeyDown("i"))
        {
            StartCoroutine(flipThing());

        }

    }


    public IEnumerator flipThing()
    {
        Debug.Log("called");
        Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
        while (rotate<179)
        {
           transform.Rotate(new Vector3(0, flip, 0) * Time.deltaTime);
           rotate = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y;
            Player.SetActive(false);
            //Debug.Log(rotate);
            rb.gravityScale = 0;
           yield return null;
        }
        
        if (rotate > 179)
        {
            Player.SetActive(true);
            Player.transform.Translate(0, 0, 0);
            rb.gravityScale = 1;
            side1.SetActive(false);
            side2.SetActive(true);
        }
    }


}
