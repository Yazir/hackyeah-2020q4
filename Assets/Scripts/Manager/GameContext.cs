using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

interface IGameContext
{
    PlayerController PlayerController { get; }
    MapManager MapManager { get; }
}

public class GameContext : MonoBehaviour, IGameContext
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Image fadeImage;

    public PlayerController PlayerController => playerController;
    public MapManager MapManager => mapManager;
    public CameraController CameraController => cameraController;

    public static GameContext instance;
    
    private bool fadingOut = false;
    private float timeElapsed;
    private bool fadingIn;
    
    private void Awake()
    {
        GameContext.instance = this;
        FadeIn();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Input.GetKey(KeyCode.B)) {
            Time.timeScale += 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.B)) {
            Time.timeScale = 1;
        }
        
        if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.B)) {
            Time.timeScale -= 0.1f;
        }

        timeElapsed += Time.deltaTime;

        if (fadingOut) {
            var color = fadeImage.color;
            color.a += 0.25f * Time.deltaTime;
            fadeImage.color = color;
            if (color.a >= 1 - Mathf.Epsilon) fadingOut = false;
        }

        if (fadingIn) {
            var color = fadeImage.color;
            color.a -= 0.25f * Time.deltaTime;
            fadeImage.color = color;
            if (color.a <= Mathf.Epsilon) fadingIn = false;
        }
    }

    public void FadeOut() {
        fadingOut = true;
        fadingIn = false;
    }

    public void FadeIn() {
        fadingOut = false;
        fadingIn = true;
    }
}
