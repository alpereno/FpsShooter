using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private  GameObject crosshair;

    [Header("Health Bar")]
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fillImage;

    Player player;

    private void Start()
    {
        // I used to event system because when GameUI screen doesn't exist player object must be executable.
        // there is no sense of in the Player script has to reference GameUI script.
        FindObjectOfType<Player>().OnDeath += onGameOver;   //event subscription 
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        setHealth(player.getHealth());
    }

    void onGameOver() {
        StartCoroutine(fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
        crosshair.SetActive(false);
    }

    IEnumerator fade(Color from, Color to, float time) {
        float speed = 1 / time;
        float percent = 0;
        while (percent<1)
        {
            percent += Time.deltaTime * speed;
            fadeImage.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    //UI Input
    public void startNewGame() {
        SceneManager.LoadScene("SampleScene");
    }

    public void setHealth(float health) {
        slider.value = health;
        fillImage.color = gradient.Evaluate(slider.normalizedValue);
    }

}
