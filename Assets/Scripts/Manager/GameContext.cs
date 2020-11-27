using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MapManager mapManager;

    public PlayerController PlayerController => playerController;
    public MapManager MapManager => mapManager;

    public static GameContext instance;
    
    private void Awake()
    {
        GameContext.instance = this;
    }
}
