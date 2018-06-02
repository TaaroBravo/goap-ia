using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Mesh notMesh;
    public Mesh nodeMesh;
    public Vector3 camera1;
    public Vector3 camera1R;
    public Vector3 camera2;
    public Vector3 camera2R;
    public Vector3 camera3;
    public Vector3 camera3R;

    private List<GameObject> allNodes = new List<GameObject>();
    public bool activeMesh;

	void Start ()
    {
        camera1 = transform.position;
        camera1R = transform.eulerAngles;
        camera2 = new Vector3(19.2f, 26.23f, -1.3f);
        camera2R = new Vector3(63.476f, 0, 0);
        camera3 = new Vector3(18.36f, 34.9f, 17.49f);
        camera3R = new Vector3(90, 0, 0);
        activeMesh = true;
    }
	
	void Update ()
    {
		if(allNodes.Count == 0)
            foreach (var item in GetComponent<NodeMatrixCreator>().allnodes)
            {
                allNodes.Add(item);
            }

        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
	}

    public void Camera1()
    {
        transform.position = camera1;
        transform.eulerAngles = camera1R;
    }

    public void Camera2()
    {
        transform.position = camera2;
        transform.eulerAngles = camera2R;
    }

    public void Camera3()
    {
        transform.position = camera3;
        transform.eulerAngles = camera3R;
    }

    public void ActiveMeshNodes()
    {
        if (activeMesh)
        {
            foreach (var item in allNodes)
            {
                item.GetComponent<MeshFilter>().mesh = notMesh;
            }
            activeMesh = false;
        }
        else
        {
            foreach (var item in allNodes)
            {
                item.GetComponent<MeshFilter>().mesh = nodeMesh;
            }
            activeMesh = true;
        }
    }
}
