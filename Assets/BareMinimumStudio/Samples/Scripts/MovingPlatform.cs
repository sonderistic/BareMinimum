using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float timeCounter;
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime * 0.5f;
        transform.position = new Vector3(Mathf.Sin(timeCounter), transform.position.y, transform.position.z);
    }
}
