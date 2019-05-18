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

    //public GameObject[] node_col = new GameObject[35];

    GameObject cur_person;

    public GameObject mcu1;
    public GameObject mcu2;
    public GameObject mcu3;

    public GameObject mcu1_temp_fire;
    public GameObject mcu2_temp_fire;
    public GameObject mcu3_temp_fire;

    public Material green_node_mat;
    public Material red_node_mat;

    // Start is called before the first frame update
    Thread udp_thread;
    Thread temp_sensor_thread;


    string prev_output = "";
    string output = "";

    float new_pos_x = 9;
    float new_pos_z = 6;

    public bool mcu1_off = false;
    public bool mcu2_off = false;
    public bool mcu3_off = false;

    void Start() {
        cur_person = GameObject.Instantiate(person_template);
        cur_person.transform.SetPositionAndRotation(new Vector3(init_pos_x, init_pos_y, init_pos_z), cur_person.transform.rotation);

        udp_thread = new Thread(new ThreadStart(start_udp));
        udp_thread.SetApartmentState(ApartmentState.STA);
        udp_thread.Start();

        temp_sensor_thread = new Thread(new ThreadStart(start_temp_sensors));
        temp_sensor_thread.SetApartmentState(ApartmentState.STA);
        temp_sensor_thread.Start();

        //new_pos_x = cur_person.transform.position.x;
        //new_pos_z = cur_person.transform.position.z;
        //GameObject.Instantiate(person_template, new Vector3(init_pos_x, init_pos_y, init_pos_z);

        mcu1_temp_fire.SetActive(false);
        mcu2_temp_fire.SetActive(false);
        mcu3_temp_fire.SetActive(false);
    }

    void start_temp_sensors() {
        while (true) {
            UdpClient mcu1_listener;
            UdpClient mcu2_listener;
            UdpClient mcu3_listener;
            IPEndPoint mcu1_end_point;
            IPEndPoint mcu2_end_point;
            IPEndPoint mcu3_end_point;
            byte[] mcu1_buffer;
            byte[] mcu2_buffer;
            byte[] mcu3_buffer;
            //Data variable

            string mcu1_ip = "127.0.4.1";
            string mcu2_ip = "127.0.2.1";
            string mcu3_ip = "127.0.3.1";

            bool ip_parse_success = IPAddress.TryParse(mcu1_ip, out IPAddress parsed_mcu1_ip);
            ip_parse_success = IPAddress.TryParse(mcu2_ip, out IPAddress parsed_mcu2_ip);
            ip_parse_success = IPAddress.TryParse(mcu3_ip, out IPAddress parsed_mcu3_ip);

            mcu1_end_point = new IPEndPoint(parsed_mcu1_ip, 2102);
            mcu1_listener = new UdpClient(2102);
            mcu2_end_point = new IPEndPoint(parsed_mcu2_ip, 2103);
            mcu2_listener = new UdpClient(2102);
            mcu3_end_point = new IPEndPoint(parsed_mcu3_ip, 2104);
            mcu3_listener = new UdpClient(2102);

            mcu1_buffer = mcu1_listener.Receive(ref mcu1_end_point);
            mcu2_buffer = mcu2_listener.Receive(ref mcu2_end_point);
            mcu3_buffer = mcu3_listener.Receive(ref mcu3_end_point);

            string mcu1_output = Encoding.ASCII.GetString(mcu1_buffer, 0, mcu1_buffer.Length);
            string mcu2_output = Encoding.ASCII.GetString(mcu2_buffer, 0, mcu2_buffer.Length);
            string mcu3_output = Encoding.ASCII.GetString(mcu3_buffer, 0, mcu3_buffer.Length);

            if (Int32.Parse(mcu1_output) < 100) {
                mcu1_off = true;
            }
            if (Int32.Parse(mcu2_output) < 100) {
                mcu2_off = true;
            }
            if (Int32.Parse(mcu3_output) < 100) {
                mcu3_off = true;
            }

            mcu1_listener.Close();
            mcu2_listener.Close();
            mcu3_listener.Close();
        }
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
    void Update() {
        if (Input.GetKeyUp(KeyCode.Return)) {
            udp_thread.Abort();
            temp_sensor_thread.Abort();

        } else if (Input.GetKeyUp(KeyCode.RightShift)) {
            udp_thread.Start();
            temp_sensor_thread.Start();
        }

        int unity_pos_x = 0;
        int unity_pos_z = 0;

        switch (new_pos_x) {
            case 0:
                unity_pos_x = 18; break;
            case 3:
                unity_pos_x = 13; break;
            case 6:
                unity_pos_x = 6; break;
            case 9:
                unity_pos_x = 0; break;
            case 12:
                unity_pos_x = -6; break;
            case 15:
                unity_pos_x = -18; break;
        }

        switch (new_pos_z) {
            case 0:
                unity_pos_z = 11; break;
            case 3:
                unity_pos_z = 6; break;
            case 6:
                unity_pos_z = 0; break;
            case 9:
                unity_pos_z = -6; break;
            case 12:
                unity_pos_z = 11; break;
        }

        cur_person.transform.position = Vector3.MoveTowards(cur_person.transform.position, new Vector3(unity_pos_x, 0, unity_pos_z), 0.2f);

        
        if (mcu1_off) {
            mcu1_temp_fire.SetActive(true);
            Collider[] hit_colliders = Physics.OverlapSphere(mcu1_temp_fire.transform.position, 15);
            foreach (var x in hit_colliders) {
                if (x.tag == "Node") {
                    x.transform.GetComponent<MeshRenderer>().material = red_node_mat;
                }
            }
        } else {
            Collider[] hit_colliders = Physics.OverlapSphere(mcu1_temp_fire.transform.position, 15);
            foreach (var x in hit_colliders) {
                if (x.tag == "Node") {
                    x.transform.GetComponent<MeshRenderer>().material = green_node_mat;
                }
            }
            mcu1_temp_fire.SetActive(false);
        }

        if (mcu2_off) {
            mcu2_temp_fire.SetActive(true);
            Collider[] hit_colliders = Physics.OverlapSphere(mcu2_temp_fire.transform.position, 15);
            foreach (var x in hit_colliders) {
                if (x.tag == "Node") {
                    x.transform.GetComponent<MeshRenderer>().material = red_node_mat;
                }
            }
        } else {
            Collider[] hit_colliders = Physics.OverlapSphere(mcu2_temp_fire.transform.position, 15);
            foreach (var x in hit_colliders) {
                if (x.tag == "Node") {
                    x.transform.GetComponent<MeshRenderer>().material = green_node_mat;
                }
            }
            mcu2_temp_fire.SetActive(false);
        }

        if (mcu3_off) {
            mcu3_temp_fire.SetActive(true);
            Collider[] hit_colliders = Physics.OverlapSphere(mcu3_temp_fire.transform.position, 15);
            foreach (var x in hit_colliders) {
                if (x.tag == "Node") {
                    x.transform.GetComponent<MeshRenderer>().material = red_node_mat;
                }
            }
        } else {
            Collider[] hit_colliders = Physics.OverlapSphere(mcu3_temp_fire.transform.position, 15);
            foreach (var x in hit_colliders) {
                if (x.tag == "Node") {
                    x.transform.GetComponent<MeshRenderer>().material = green_node_mat;
                }
            }
            mcu3_temp_fire.SetActive(false);
        }
    }
}
