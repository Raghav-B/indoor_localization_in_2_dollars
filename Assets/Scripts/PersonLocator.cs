using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class PersonLocator : MonoBehaviour
{
    public GameObject person_template;
    public float init_pos_x = 0f;
    public float init_pos_y = 0f;
    public float init_pos_z = 0f;
    public float movement_speed = 2f;

    GameObject cur_person;
    
    // Start is called before the first frame update
    void Start()
    {
        cur_person = GameObject.Instantiate(person_template);
        cur_person.transform.SetPositionAndRotation(new Vector3(init_pos_x, init_pos_y, init_pos_z), cur_person.transform.rotation);


        //GameObject.Instantiate(person_template, new Vector3(init_pos_x, init_pos_y, init_pos_z);
    }

    // Update is called once per frame
    void Update()
    {
        //whenever update signal is sent, perform a graphical update
        if (Input.GetKey(KeyCode.Space)) {
            int new_pos_x = 10;
            int new_pos_z = 10;
            cur_person.transform.position = Vector3.MoveTowards(cur_person.transform.position, new Vector3(new_pos_x, 0, new_pos_z), 0.2f);
        }
    }
}
