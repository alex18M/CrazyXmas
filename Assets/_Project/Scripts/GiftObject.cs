using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GiftObject : MonoBehaviour
{
    public GiftPropertiesSO giftProperties;
    public bool hasCollided = false;
    public Transform particlesSpawnPoint;
    
    private float _maxHealthCandy = 10f;

    public ParticleSystem[] particleSystems;

    private void Start()
    {
        // Desactivar todos los sistemas de partículas al inicio
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Stop();
            particleSystem.gameObject.SetActive(false);
        }
        ResetHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeliveryZone") && !hasCollided)
        {
            // Realizar acciones basadas en la primera colisión
            hasCollided = true;

            // Elegir aleatoriamente un sistema de partículas y activarlo en el punto específico
            int randomIndex = Random.Range(0, particleSystems.Length);

            if (particlesSpawnPoint != null)
            {
                // Establecer la posición y rotación de las partículas según el punto de spawn
                particleSystems[randomIndex].transform.position = particlesSpawnPoint.position;
                particleSystems[randomIndex].transform.rotation = particlesSpawnPoint.rotation;
            }

            // Reproducir las partículas
            particleSystems[randomIndex].gameObject.SetActive(true);
            particleSystems[randomIndex].Play();
            // Desactivar el objeto de regalo
            //gameObject.SetActive(false);
            Destroy(gameObject, 3f);
        }
    }

    
    public void ResetHealth()
    {
        giftProperties.health = giftProperties.type switch
        {
            "Comun" => 20f,
            "Epic" => 30f,
            "Rare" => 40f,
            "Special" => 50f,
            _ => _maxHealthCandy
        };
    }
    
}