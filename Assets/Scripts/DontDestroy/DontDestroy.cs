using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    static bool HasBeenInstantied = false;
    // Start is called before the first frame update
    void Start()
    {
        if (HasBeenInstantied)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        HasBeenInstantied = true;
    }
}
