using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePortalA : MonoBehaviour
{
    private GameObject portal;
    // Start is called before the first frame update
    void Start()
    {
        portal = GameObject.Find("Portal A");
        portal.transform.position = transform.position;
        portal.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
