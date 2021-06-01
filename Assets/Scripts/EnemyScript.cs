using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField]
    private float enemySpeed = 5f;
    [SerializeField]
    private GameObject player, enemyLaser;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }

    private void Start()
    {
        StartCoroutine("FireLaser");
    }

    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);

        //Check for bottom of screen
        if(transform.position.y < -2)
        {
            transform.position = new Vector3(Random.Range(-9, 9), 11, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "laser")
        {
            GetComponent<Collider2D>().enabled = false;
            if(other.transform.parent)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
            if(player != null)
            {
                player.GetComponent<PlayerScript>().UpdateScore();
            }
            DestroyEnemy();
        }
        
    }

    public void DestroyEnemy()
    {
        enemySpeed = .5f;
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("OnEnemyDestroyed");
        Destroy(gameObject,1.5f);
    }

    IEnumerator FireLaser()
    {
        while (player != null)
        {
            yield return new WaitForSeconds(Random.Range(3f, 4f));
            Vector3 pos = new Vector3(transform.position.x, transform.position.y - 2.68f, 0f);
            GameObject eL = GameObject.Instantiate(enemyLaser, pos, Quaternion.identity);
            eL.tag = "enemyLaser";
            int l = eL.transform.childCount;
            for(int i = 0; i < l; i++)
            {
                eL.transform.GetChild(i).tag = "enemyLaser";
            }
        }
    }
}
