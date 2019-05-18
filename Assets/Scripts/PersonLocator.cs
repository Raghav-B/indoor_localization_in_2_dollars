using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class PersonLocator : MonoBehaviour
{
    public GameObject person_template;
    public float init_pos_x = 0f;
    public float init_pos_y = 0f;
    public float init_pos_z = 0f;
    public float movement_speed = 2f;

    GameObject cur_person;

    // Start is called before the first frame update
    Thread udp_thread;


    string prev_output = "";
    string output = "";

    float new_pos_x = 0;
    float new_pos_z = 0;

    void Start()
    {
        cur_person = GameObject.Instantiate(person_template);
        cur_person.transform.SetPositionAndRotation(new Vector3(init_pos_x, init_pos_y, init_pos_z), cur_person.transform.rotation);

        udp_thread = new Thread(new ThreadStart(start_udp));
        udp_thread.SetApartmentState(ApartmentState.STA);
        udp_thread.Start();

        new_pos_x = cur_person.transform.position.x;
        new_pos_z = cur_person.transform.position.z;
        //GameObject.Instantiate(person_template, new Vector3(init_pos_x, init_pos_y, init_pos_z);
    }

    void start_udp() {
        while (true) {
            UdpClient server_listener;
            IPEndPoint server_end_point;
            byte[] buffer;
            //Data variable

            string ip_to_listen = "127.0.0.1";
            bool ip_parse_success = IPAddress.TryParse(ip_to_listen, out IPAddress parsed_listening_ip);

            server_end_point = new IPEndPoint(parsed_listening_ip, 3000);
            server_listener = new UdpClient(3000);

            buffer = server_listener.Receive(ref server_end_point);

            output = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            if (prev_output != output) {
                new_pos_x = Int32.Parse(output.Substring(0, 3));
                new_pos_z = Int32.Parse(output.Substring(4, 3));
            }
            prev_output = Encoding.ASCII.GetString(buffer, 0, buffer.Length);

            Debug.Log(output);

            server_listener.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return)) {
            udp_thread.Abort();
        } else if (Input.GetKeyUp(KeyCode.RightShift)) {
            udp_thread.Start();
        }

        cur_person.transform.position = Vector3.MoveTowards(cur_person.transform.position, new Vector3(new_pos_x, 0, new_pos_z), 0.2f);

    }
}
