using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public PlayerController PlayerController => playerController;
    public MapManager MapManager => mapManager;
    public CameraController CameraController => cameraController;

    public static GameContext instance;
    
    private void Awake()
    {
        GameContext.instance = this;
    }
}
