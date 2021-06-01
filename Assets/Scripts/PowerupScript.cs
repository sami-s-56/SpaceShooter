using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 3.5f);
        if(transform.position.y < -1)
        {
            Destroy(gameObject);
        }
    }
}
