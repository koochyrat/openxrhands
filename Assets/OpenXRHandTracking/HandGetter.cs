using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
public class HandGetter : MonoBehaviour
{
    public HandTrackingFeature.Hand_Index hand;
    //GameObject lhand,rhand;

    public float rx,ry,rz;
    public Material mat;
    public Vector3[] positions;
    public Quaternion[] orientations;
    public Transform[] joints;
    public float[] radius;

    private struct BoneLineData
    {
        public LineRenderer r;
        public Vector3[] vertices;
        public int[] indices;
    }

    //line data for finger line renderers. indices are indices into OpenXR positions array
    private BoneLineData[] boneLines = new BoneLineData[]
    {
        new BoneLineData { vertices = new Vector3[2], indices = new int[] { 1, 0 } },   //palm
        new BoneLineData { vertices = new Vector3[5], indices = new int[] { 1, 2, 3, 4, 5 } },  //thumb
        new BoneLineData { vertices = new Vector3[6], indices = new int[] { 1, 6, 7, 8, 9, 10 } },  //index
        new BoneLineData { vertices = new Vector3[6], indices = new int[] { 1, 11, 12, 13, 14, 15 } },  //middle
        new BoneLineData { vertices = new Vector3[6], indices = new int[] { 1, 16, 17, 18, 19, 20 } },  //ring
        new BoneLineData { vertices = new Vector3[6], indices = new int[] { 1, 21, 22, 23, 24, 25 } }   //pinky
    };

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < boneLines.Length; i++)
        {
            GameObject lineObj = new GameObject("Line");
            BoneLineData bld = boneLines[i];
            bld.r = lineObj.AddComponent<LineRenderer>();
            bld.r.startWidth = 0.01f;
            bld.r.endWidth = 0.01f;
            bld.r.sharedMaterial = mat;
            bld.r.positionCount = bld.vertices.Length;
            boneLines[i] = bld;
        }
    }

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
            //float[] radius;
            hf.GetHandJoints(hand, out positions, out orientations, out radius);
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
            //draw lines in game view
            for(int i = 0; i < boneLines.Length; i++)
            {
                BoneLineData bld = boneLines[i];
                for(int v = 0; v < bld.vertices.Length; v++) { bld.vertices[v] = positions[bld.indices[v]]; }
                bld.r.SetPositions(bld.vertices);
                boneLines[i] = bld;
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
