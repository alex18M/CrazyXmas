using System;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    public PlayerPointsManagerSO playerPointsManagerSO;
    [SerializeField] private TextMeshProUGUI pointsText;

    private void Awake()
    {
        Instance = this;
        playerPointsManagerSO.puntos = 0;
    }

    private void Start()
    {
        if (playerPointsManagerSO == null)
        {
            Debug.LogError("PlayerPointsManager no asignado en el Inspector.");
        }
    }

    // Lógica para sumar puntos
    public void AddPoints(int cantidad)
    {
        playerPointsManagerSO.puntos += cantidad;
        pointsText.text = "Score: " + playerPointsManagerSO.puntos;
        Debug.Log("Puntos actuales del jugador: " + playerPointsManagerSO.puntos);
    }
}