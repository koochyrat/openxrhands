using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
public class HandGetter : MonoBehaviour
{

    GameObject lhand,rhand;

    // Start is called before the first frame update
    void Start()
    {
    }

    public float rx,ry,rz;
    public Material mat;
    public Vector3[] positions;
    public Quaternion[] orientations;
    public Transform[] joints;
    // Update is called once per frame
    void Update()
    {
        HandTrackingFeature hf=OpenXRSettings.Instance.GetFeature<HandTrackingFeature>();
        if(hf==null || hf.enabled==false)
        {
            print("You need to enable the openXR hand tracking support extension ");
        }
        
        if(hf)
        {
            float[] radius;
            hf.GetHandJoints(HandTrackingFeature.Hand_Index.R, out positions, out orientations, out radius);
            if (positions.Length == 0) return;
            if(joints == null || joints.Length == 0)
            {
                joints = new Transform[positions.Length];
                for(int i = 0; i < joints.Length;i++)
                {
                    joints[i] = new GameObject("Joint").transform;
                    joints[i].parent = transform;
                }
            }
            for (int i = 0; i < positions.Length; i++)
            {
                joints[i].transform.position = positions[i];
                joints[i].transform.rotation = orientations[i];
                switch (i)
                {
                    case 1: //wrist
                        transform.position = joints[i].transform.position;
                        transform.rotation = joints[i].transform.rotation;
                        break;
                    case 0: //palm
                        Debug.DrawLine(joints[1].transform.position, joints[i].transform.position, Color.red);
                        break;
                    case 2: case 6: case 11: case 16: case 21:  //metacarpals
                        Debug.DrawLine(joints[1].transform.position, joints[i].transform.position, Color.green);
                        break;
                    default:
                        Debug.DrawLine(joints[i - 1].transform.position, joints[i].transform.position, Color.blue);
                        break;
                }
            }

        }
        /*if(lhand==null)
        {
            hf.GetHandMesh(HandTrackingFeature.Hand_Index.L,transform,mat,out lhand);
        }
        else
        {
            hf.ApplyHandJointsToMesh(HandTrackingFeature.Hand_Index.L,lhand,rx,ry,rz);
        }    
        if(rhand==null)
        {
            hf.GetHandMesh(HandTrackingFeature.Hand_Index.R,transform,mat,out rhand);            
        }else
        {
            hf.ApplyHandJointsToMesh(HandTrackingFeature.Hand_Index.R,rhand,rx,ry,rz);
        }*/
    }
}
