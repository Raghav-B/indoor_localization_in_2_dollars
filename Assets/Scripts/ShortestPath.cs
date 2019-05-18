using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShortestPath : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData click) {
        if (click.button == PointerEventData.InputButton.Left) {
            start_fake_sssp();
        }
    }

    public GameObject[] nodes_col = new GameObject[35];
    public GameObject start_node;
    public int start_node_index = 34;
    public GameObject end_node;
    public int end_node_index;
    GameObject cur_node;
    public int cur_node_index;

    public Material red_node_mat;
    public Material green_node_mat;

    List<int> path_indexes = new List<int>;

    void dfs(int append_node) {
        //bool path_found = false;
        if (cur_node_index - 7 >= 0) {
            if (nodes_col[cur_node_index - 7].transform.GetComponent<MeshRenderer>().material == green_node_mat) { //Up
                cur_node = nodes_col[cur_node_index - 7];
                cur_node_index -= 7;
                //path_found = true;
                dfs(cur_node_index);
            }
        }

        if ((cur_node_index - 1) % 7 == 0) {
            if (nodes_col[cur_node_index - 1].transform.GetComponent<MeshRenderer>().material == green_node_mat) { //Right
                cur_node = nodes_col[cur_node_index - 1];
                cur_node_index -= 1;
                //path_found = true;
                dfs(cur_node_index);
            }
        }

        if (cur_node_index + 7 <= 28) {
            if (nodes_col[cur_node_index + 7].transform.GetComponent<MeshRenderer>().material == green_node_mat) { //Down
                cur_node = nodes_col[cur_node_index + 7];
                cur_node_index += 7;
                //path_found = true;
                dfs(cur_node_index);
            }
        }

        if ((cur_node_index + 1) % 7 >= 0) {
            if (nodes_col[cur_node_index + 1].transform.GetComponent<MeshRenderer>().material == green_node_mat) { //Left
                cur_node = nodes_col[cur_node_index + 7];
                cur_node_index += 1;
                //path_found = true;
                dfs(cur_node_index);
            }
        }

        if (append_node == end_node_index) {
            path_indexes.Add(end_node_index);
            end_node_index = append_node;
            return;
        } else {
            return;
        }
    }

    void start_fake_sssp() {
        cur_node = start_node;
        cur_node_index = start_node_index;
        dfs(cur_node_index);

        foreach (var x in path_indexes) {
            Debug.Log(x.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
