using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;

public class ShortestPath : MonoBehaviour, IPointerClickHandler
{
    //Thread sssp_thread;

    public void OnPointerClick(PointerEventData click) {
        if (click.button == PointerEventData.InputButton.Left) {
            //sssp_thread = new Thread(new ThreadStart(start_fake_sssp));
            //sssp_thread.SetApartmentState(ApartmentState.STA);
            //sssp_thread.Start();
            start_fake_sssp();
        }
    }

    public GameObject[] nodes_col = new GameObject[35];
    public GameObject start_node;
    public int start_node_index = 34;
    public GameObject end_node;
    public int end_node_index = 0;
    GameObject cur_node;
    public int cur_node_index = 34;

    public Material red_node_mat;
    public Material green_node_mat;
    public Material node_visited_mat;

    List<int> path_indexes = new List<int>();
    List<string> maze = new List<string>();

    void start_fake_sssp() {
        int index = 0;
        int string_index = 0;
        string temp_string = "";
        foreach (var x in nodes_col) {
            if (x.transform.GetComponent<Text>().text == "") {
                if (index == 0) {
                    temp_string += 'A';
                } else if (index == 34) {
                    temp_string += 'F';
                } else { 
                    temp_string += 'G';
                }
            } else {
                if (index == 34) {
                    temp_string += 'A';
                } else {
                    temp_string += 'R';
                }
            }
            string_index++;
            index++;

            if (string_index == 7) {
                maze.Add(temp_string);
                string_index = 0;
                temp_string = "";
            }
        }
        //maze.Reverse();

        for (int i = 0; i < 7; i++) {
            Debug.Log(maze[i]);
        }
    }

    void dfs() {

    }

    /*void start_fake_sssp() {
        int string_index = 0;
        string temp_string = "";
        foreach (var x in nodes_col) {
            if (x.transform.GetComponent<Text>().text == "") {
                temp_string += 'G';
            } else {
                temp_string += 'R';
            }
            string_index++;

            if (string_index == 7) {
                maze.Add(temp_string);
                string_index = 0;
                temp_string = "";
            }
        }

        temp_maze6 = maze[6];
        temp_maze6[4] = 
        maze[6] = maze[6].ToString()[4]
        //maze[0][0] = 'A';

        cur_node = start_node;
        cur_node_index = start_node_index;
        dfs(cur_node_index);

        foreach (var x in path_indexes) {
            Debug.Log(x.ToString());
        }

        GameObject[] restore_list = GameObject.FindGameObjectsWithTag("Node");
        foreach (var x in restore_list) {
            if (x.transform.GetComponent<Text>().text != "R") {
                x.transform.GetComponent<Text>().text = "";
            }
        }

        //sssp_thread.Abort();
    }*/
}
