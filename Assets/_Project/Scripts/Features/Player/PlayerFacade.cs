using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerFacade : MonoBehaviour
{
    private PlayerBase _base;
    private PlayerHealth _health;
    private PlayerVisual _visual;
    private PlayerHUD _hud;

    public PlayerBase Base => _base;
    public PlayerHealth Health => _health;
    public PlayerVisual Visual => _visual;
    public PlayerHUD Hud => _hud;

    private void Awake()
    {
        _base = GetComponent<PlayerBase>();
        _health = GetComponent<PlayerHealth>();
        _visual = GetComponent<PlayerVisual>();
        _hud = GetComponentInChildren<PlayerHUD>();
    }
}
