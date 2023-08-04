using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProbeControl : MonoBehaviour
{
#if UNITY_EDITOR

    public LightProbeGroup lp;

    public int Width, Height;
    public float DeltaW, DeltaH;

    void Start()
    {
        lp = GetComponent<LightProbeGroup>();

    }

    [ContextMenu("Init Light Probe")]
    void InitLightProbe()
    {
        int nLightProbs = Width * Height;

        Vector3[] array = new Vector3[nLightProbs];

        for (int i = 0; i < nLightProbs; i++)
        {
            int x = i % Width;
            int z = i / Width;
            array[i] = new Vector3(x * DeltaW, 0, z * DeltaH);
        }

        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        array[x * width + y] = new vector3(x * deltaw, 0, y * deltah);
        //    }
        //}

        lp.probePositions = array;
    }

#endif
}
