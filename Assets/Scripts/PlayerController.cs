using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            // print Hello world
            Debug.Log("Hello, World!");

        }
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0, v) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
}
