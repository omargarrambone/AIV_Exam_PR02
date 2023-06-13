using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static GameObject PlayerGameObject { get; private set; }
    void Start()
    {
       if(PlayerGameObject == null) PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    static public void SetPosition(Vector3 newPosition)
    {
        PlayerGameObject.transform.position = newPosition;
    }

    static public void SetPosition(int x,int y,int z)
    {
        PlayerGameObject.transform.position = new Vector3(x,y,z);
    }
}
