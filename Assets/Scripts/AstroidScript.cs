using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidScript : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed = 5f;
    [SerializeField]
    private GameObject explosionPrefab;
    private GameObject spawnManager;
    void Update()
    {
        transform.Rotate(0, 0, -rotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "laser")
        {
            spawnManager = GameObject.Find("SpawnManager");
            GameObject a = GameObject.Instantiate(explosionPrefab, transform);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(other.gameObject);
            Destroy(a, 3f);
            spawnManager.GetComponent<SpawnerScript>().StartSpawning();
            Destroy(gameObject,3f);
        }
    }
}
