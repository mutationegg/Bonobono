using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public string levelToLoad;
    public string levelToUnlock;

    private PlayerController thePlayer;
    private CameraController theCamera;
    private LevelManager theLevelManager;

    public float waitToMove;
    public float waitToLode;

    private bool movePlayer;

    public Sprite flagOpen;

    private SpriteRenderer theSpriteRender;

    //public float nextStageDelay;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraController>();
        theLevelManager = FindObjectOfType<LevelManager>();

        theSpriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movePlayer)
        {
            thePlayer.myRigidbody.velocity = new Vector3(thePlayer.moveSpeed, thePlayer.myRigidbody.velocity.y, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //SceneManager.LoadScene(levelToLoad);

            theSpriteRender.sprite = flagOpen;

            StartCoroutine("LevelEndCo");
        }
    }

    public IEnumerator LevelEndCo()
    {
        thePlayer.canMove = false;
        theCamera.followTarget = false;
        theLevelManager.invincible = true;

        theLevelManager.levelMusic.Stop();
        theLevelManager.gameOverMusic.Play();

        thePlayer.myRigidbody.velocity = Vector3.zero;

        PlayerPrefs.SetInt("CoinCount", theLevelManager.coinCount);
        PlayerPrefs.SetInt("PlayerLives", theLevelManager.currentLives);

        PlayerPrefs.SetInt(levelToUnlock, 1);

        yield return new WaitForSeconds(waitToMove);

        movePlayer = true;

        yield return new WaitForSeconds(waitToLode);

        SceneManager.LoadScene(levelToLoad);
    }

}
