using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives = 1;

    public float invincibleTime = 1.0f;
    public bool isInvincible = false;
    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Misslie"))
        {
            currentLives--;
            Destroy(other.gameObject);
        }
    }

    public void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestarGame", 3.0f);

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
