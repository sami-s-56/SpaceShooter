using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab, enemyParent, astroidPrefab; 
    private GameObject player;

    [SerializeField]
    private List<GameObject> spawnObjects;

    private void Start()
    {
        GameManagerScript.GameStarted += OnStart;
        GameManagerScript.GameReset += OnStart;
        GameManagerScript.GameOver += OnGameOver;
    }

    private void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("player");
        GameObject.Instantiate(astroidPrefab, new Vector3(0, 5, 0), Quaternion.identity);
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private void OnGameOver()
    {
        StopAllCoroutines();
        for(int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (player!=null)
        {
            GameObject e = Instantiate(enemyPrefab, new Vector3(Random.Range(-9,9),9,0), Quaternion.identity);
            e.transform.SetParent(enemyParent.transform);
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (player != null)
        {
            yield return new WaitForSeconds(Random.Range(7,12));
            GameObject o = spawnObjects[Random.Range(0, spawnObjects.Count())];
            Instantiate(o, new Vector3(Random.Range(-9, 9), 9, 0), Quaternion.identity);
        }
    }
}
