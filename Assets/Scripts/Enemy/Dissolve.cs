using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{

    //public GameObject Enemy;

    public SkinnedMeshRenderer EnemyMeshRenderer;
    //public MeshRenderer PouchRenderer;
    //public MeshRenderer SwordCaseRenderer;
    //public MeshRenderer KatanaRenderer;

    public Material DissolveMaterial;

    public bool isMaterialSet;

    bool isDissolving;
    float t = -1;
    // Start is called before the first frame update
    void Start()
    {
        //MeshRenderer.material = material;
        EnemyMeshRenderer.material = DissolveMaterial;

    }

    // Update is called once per frame
    void Update()
    {
        //DissolveEnemy();
        //if (!isMaterialSet)
        //{
        //    //SetDissolveMaterial();
        //    EnemyMeshRenderer.material = DissolveMaterial;


        //    isMaterialSet = true;
        //}


        if (isMaterialSet)
        {
            isDissolving = true;
        }

        if (isDissolving)
        {
            t += Time.deltaTime;

            if (t >= 1)
            {
                t = -1;
                isDissolving = false;
                isMaterialSet = false;

            }
        }


        DissolveMaterial.SetFloat("_Edge_Width", t);
    }


    //public void SetDissolveMaterial()
    //{
    //    EnemyMeshRenderer.material = DissolveMaterial;
    //    //PouchRenderer.material = DissolveMaterial;
    //    //SwordCaseRenderer.material = DissolveMaterial;
    //    //KatanaRenderer.material = DissolveMaterial;
    //    //isMaterialSet = true;
    //}


    public void DissolveEnemy()
    {
        if (!isMaterialSet)
        {
            //SetDissolveMaterial();
            EnemyMeshRenderer.material = DissolveMaterial;


            isMaterialSet = true;
        }

        if (isMaterialSet)
        {
            isDissolving = true;
        }

        if (isDissolving)
        {
            t += Time.deltaTime;

            if (t >= 1)
            {
                t = -1;
                isDissolving = false;
                isMaterialSet = false;
                
            }
        }

        
        DissolveMaterial.SetFloat("_Edge_Width", t);
    }

    //IEnumerator DissolveCoroutine()
    //{
    //    yield return new WaitForSeconds(1f);
    //    float t = -1;
    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime;
    //        MeshRenderer.material("Dissolve")
            
    //    }
    //}
}
