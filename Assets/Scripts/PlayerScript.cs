using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField]
    private float forwardSpeed = 5f;
    [SerializeField]
    private int scoreIncrement = 10;
    private int score = 0;

    [SerializeField]
    private GameObject laserPrefab, trippleshotPrefab, shieldAnim, explosionPrefab;
    private GameObject uiManager;

    [SerializeField]
    private float rateOfFire = .25f;
    private float nextFireTime = -1;

    private int lives = 3;
    [SerializeField]
    private bool isTrippleShot = false;
    private bool isShieldActive = false;

    [SerializeField]
    private List<GameObject> engineFailure;

    [SerializeField]
    private AudioClip laserClip, powerupClip;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        uiManager = GameObject.Find("UIManager");
    }

    void Update()
    {
        //Inputs
        ProcessInput();

        //Bounds Check
        BoundsCheck();

    }

    private void ProcessInput()
    {
        //Player Inputs
        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * forwardSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * forwardSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFireTime)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        if (isTrippleShot)
        {
            nextFireTime = Time.time + rateOfFire;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(trippleshotPrefab, pos, Quaternion.identity);
        }
        else
        {
            nextFireTime = Time.time + rateOfFire;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
            Instantiate(laserPrefab, pos, Quaternion.identity);
        }
        GetComponent<AudioSource>().clip = laserClip;
        GetComponent<AudioSource>().Play();
    }

    private void BoundsCheck()
    {
        //Bound Player to specific area

        //Side Bounds
        if (transform.position.x > 9)
        {
            transform.position = new Vector3(9f, transform.position.y);
        }
        else if (transform.position.x < -9)
        {
            transform.position = new Vector3(-9f, transform.position.y);
        }

        //Forward Bounds
        if (transform.position.y > 3)
        {
            transform.position = new Vector3(transform.position.x, 3f);
        }
        else if (transform.position.y < -0.75)
        {
            transform.position = new Vector3(transform.position.x, -0.75f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            if (!isShieldActive)
            {
                lives--;
                if (lives > 0)
                {
                    int engineSelector = Random.Range(0, engineFailure.Count());
                    
                    engineFailure[engineSelector].SetActive(true);
                    engineFailure.RemoveAt(engineSelector);
                }
                uiManager.GetComponent<UIManagerScript>().UpdateLives(lives);
            }
            other.transform.GetComponent<EnemyScript>().DestroyEnemy();
            if (lives == 0)
            {
                StartCoroutine("GameOverRoutine");
            }
        }
        else if(other.tag == "enemyLaser")
        {
            print("Collided with Laser");
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            if (!isShieldActive)
            {
                lives--;
                if (lives > 0)
                {
                    int engineSelector = Random.Range(0, engineFailure.Count());

                    engineFailure[engineSelector].SetActive(true);
                    engineFailure.RemoveAt(engineSelector);
                }
                uiManager.GetComponent<UIManagerScript>().UpdateLives(lives);
            }
            if (other.transform.parent)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
            if (lives == 0)
            {
                StartCoroutine("GameOverRoutine");
            }
        }
        else if (other.tag == "trippleshot")
        {
            GetComponent<AudioSource>().clip = powerupClip;
            GetComponent<AudioSource>().Play();
            if (isTrippleShot)
            {
                StopCoroutine("EnableTrippleShot");
            }
            StartCoroutine("EnableTrippleShot");
            Destroy(other.gameObject);
        }
        else if (other.tag == "speed")
        {
            GetComponent<AudioSource>().clip = powerupClip;
            GetComponent<AudioSource>().Play();
            if (forwardSpeed == 10f)
            {
                StartCoroutine(EnableSpeedBoost());
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "shield")
        {
            GetComponent<AudioSource>().clip = powerupClip;
            GetComponent<AudioSource>().Play();
            if (isShieldActive)
            {
                StopCoroutine("ActivateShieldRoutine");
            }
            StartCoroutine("ActivateShieldRoutine");
            Destroy(other.gameObject);
        }
    }

    private IEnumerator GameOverRoutine()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        for (int i=0; i<transform.childCount; i++)
        {
            GameObject Childs = transform.GetChild(i).gameObject;
            Childs.SetActive(false);
        }
        GameObject e = GameObject.Instantiate(explosionPrefab, transform);
        yield return new WaitForSeconds(2.5f);
        GameManagerScript.GameOver();
        Destroy(e);
        Destroy(gameObject);
    }

    private IEnumerator EnableTrippleShot()
    {
        isTrippleShot = true;
        yield return new WaitForSeconds(10f);
        isTrippleShot = false;
    }

    private IEnumerator ActivateShieldRoutine()
    {
        isShieldActive = true;
        shieldAnim.SetActive(true);
        yield return new WaitForSeconds(10f);
        isShieldActive = false;
        shieldAnim.SetActive(false);
    }

    private IEnumerator EnableSpeedBoost()
    {
        float normalSpeed = forwardSpeed;
        forwardSpeed = forwardSpeed * 1.5f;
        yield return new WaitForSeconds(10f);
        forwardSpeed = normalSpeed;
    }

    public void UpdateScore()
    {
        score += scoreIncrement;
        uiManager.GetComponent<UIManagerScript>().UpdateScore(score);
    }

    public int GetScore()
    {
        return score;
    }

}
