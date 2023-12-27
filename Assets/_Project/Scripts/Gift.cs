using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{

    [SerializeField] private GiftPropertiesSO giftType;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private ParticleSystem explosionParticles;
    
    
    private bool canReceiveDamage = false;
    private void Start()
    {
        ResetGiftLife();
    
        UpdateHealthBar();
        
        // Después de un tiempo específico, permitir que el regalo reciba daño
        StartCoroutine(EnableDamageAfterDelay(2f));
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (canReceiveDamage)
        {
            float fallForce = collision.relativeVelocity.magnitude;
            float damage = CalculateDamage(fallForce, collision.contacts[0].point);
            ApplyDamage(damage);
            UpdateHealthBar();

            if (giftType.health <= 0)
            {
                // El regalo ha llegado a cero de vida
                ExplodeAndDestroy();
            }
        }
    }

    private float CalculateDamage(float fallForce, Vector3 ImpactPoint)
    {
        // Ejemplo: Cuanto más fuerte cae, más daño
        float DamageBasedOnStrength = fallForce * 0.1f;

        // Ejemplo: Cuanto más alto impacta en el objeto, más daño
        float damageBasedOnHeight = Mathf.Abs(ImpactPoint.y - transform.position.y) * 0.5f;

        // Suma de ambos factores de daño
        float totaldamage = DamageBasedOnStrength + damageBasedOnHeight;

        return totaldamage;
    }

    private void ApplyDamage(float amountDamage)
    {
        // Restar la cantidad de daño de la vida del objeto
        giftType.health -= amountDamage;
        
        // bar.fillAmount = giftType.health / 100f;
    
    }
    
    private void ExplodeAndDestroy()
    {
        // Instanciar las partículas de explosión en la posición del regalo
        if (explosionParticles != null)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
        }

        // Destruir el objeto regalo
        Destroy(gameObject);
    }
    
    private void UpdateHealthBar()
    {
        Image bar = healthBar.GetComponent<Image>();
        
        // Asegúrate de que giftType tenga un valor inicial para la vida y no sea null
        if (giftType != null)
        {
            // Asegúrate de que la barra de vida tenga un componente Image adjunto
            if (bar != null)
            {
                // Actualiza la barra de vida basándote en el valor de la vida actual del regalo
                bar.fillAmount = giftType.health / giftType.maxHealth;
            }
            else
            {
                Debug.LogError("HealthBarFill is not assigned!");
            }
        }
        else
        {
            Debug.LogError("GiftType is not assigned!");
        }
    }
    
    private void ResetGiftLife()
    {
        // Reinicia la vida del regalo al valor máximo
        giftType.health = giftType.maxHealth;
        
        UpdateHealthBar();
    }
    
    private IEnumerator EnableDamageAfterDelay(float delay)
    {
        // Esperar el tiempo especificado antes de permitir que el regalo reciba daño
        yield return new WaitForSeconds(delay);

        // Activar la lógica de daño
        canReceiveDamage = true;
    }
}