using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakaRemove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    float destroybakatime = 3;
    // Update is called once per frame
    void Update()
    {
        destroybakatime -= Time.deltaTime;
        if (destroybakatime <= 0)
        {
            Destroy(gameObject);
        }
    }
}