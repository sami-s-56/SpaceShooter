using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    void Update()
    {
        if(gameObject.tag == "laser")
        {
            playerLaserMovement();
        }
        else if(gameObject.tag == "enemyLaser")
        {
            enemyLaserMovement();
        }
    }

    private void playerLaserMovement()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if (transform.position.y > 10)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void enemyLaserMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);

        if (transform.position.y < -5)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
