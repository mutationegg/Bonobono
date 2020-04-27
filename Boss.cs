using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool bossActive;

    public float timeBetweenDrops;
    private float timeBetweenDropStore;
    private float dropCount;

    public float waitForPlatforms;
    private float platformsCount;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform dropSawSpawnPoint;

    public GameObject dropSaw;

    public GameObject theBoss;
    public bool bossRight;

    public GameObject rightPlatforms;
    public GameObject leftPlatforms;

    public bool takeDamage;

    public int startingHealth;
    private int currentHealth;

    public GameObject levelExit;

    private CameraController theCamera;

    public LevelManager theLevelManager;

    public bool waitingForRespawn;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenDropStore = timeBetweenDrops;
        dropCount = timeBetweenDrops;
        platformsCount = waitForPlatforms;

        theBoss.transform.position = rightPoint.position;
        bossRight = true;

        currentHealth = startingHealth;
        theCamera = FindObjectOfType<CameraController>();
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theLevelManager.respawnCoActive)
        {
            bossActive = false;
            waitingForRespawn = true;
        }

        if(!theLevelManager.respawnCoActive && waitingForRespawn)
        {
            theBoss.SetActive(false);
            leftPlatforms.SetActive(false);
            rightPlatforms.SetActive(false);

            timeBetweenDrops = timeBetweenDropStore;

            platformsCount = waitForPlatforms;
            dropCount = timeBetweenDrops;

            theBoss.transform.position = rightPoint.position;
            bossRight = true;

            currentHealth = startingHealth;

            theCamera.followTarget = true;
        }

        if (bossActive)
        {
            theCamera.followTarget = false;
            theCamera.transform.position = Vector3.Lerp(theCamera.transform.position, new Vector3(transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z), theCamera.smooting *Time.deltaTime);

            theBoss.SetActive(true);

            if(dropCount > 0)
            {
                dropCount -= Time.deltaTime;
            }
            else
            {
                dropSawSpawnPoint.position = new Vector3(Random.Range(leftPoint.position.x, rightPoint.position.x), dropSawSpawnPoint.position.y, dropSawSpawnPoint.position.z);
                Instantiate(dropSaw, dropSawSpawnPoint.position, dropSawSpawnPoint.rotation);
                dropCount = timeBetweenDrops;
            }

            if (bossRight)
            {
                if(platformsCount > 0)
                {
                    platformsCount -= Time.deltaTime;
                }
                else
                {
                    rightPlatforms.SetActive(true);
                }
            }
            else
            {
                if (platformsCount > 0)
                {
                    platformsCount -= Time.deltaTime;
                }
                else
                {
                    leftPlatforms.SetActive(true);
                }
            }

            if (takeDamage)
            {
                currentHealth -= 1;

                if(currentHealth <= 0)
                {
                    theCamera.followTarget = true;

                    levelExit.SetActive(true);
                    gameObject.SetActive(false);
                }

                if (bossRight)
                {
                    theBoss.transform.position = leftPoint.position;
                }
                else
                {
                    theBoss.transform.position = rightPoint.position;
                }
                bossRight = !bossRight;

                rightPlatforms.SetActive(false);
                leftPlatforms.SetActive(false);

                platformsCount = waitForPlatforms;

                timeBetweenDrops = timeBetweenDrops / 2f;

                takeDamage = false;
            }

            //if (theLevelManager.checkBoss)
            //{
            //    theBoss.SetActive(false);
            //    theLevelManager.checkBoss = false;
            //}


        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            bossActive = true;
        }
    }

}
