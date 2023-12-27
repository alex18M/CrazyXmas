using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform target; // Referencia al transform del personaje principal
    public float speed = 3f; // Velocidad de movimiento del enemigo
    public int maxHealth = 2; // Máxima salud del enemigo
    public float detectionRadius = 5f;
    public GameObject giftPrefab; // Prefab del regalo a lanzar
    public Transform throwPoint; // Punto desde el cual se lanzarán los regalos
    public float throwForce = 10f; // Fuerza con la que se lanzarán los regalos
    public float dashSpeed = 10f; // Velocidad del dash

    private int currentHealth; // Salud actual del enemigo
    private NavMeshAgent navMeshAgent;
    private bool isFollowingObject = false;
    private Transform targetObject;
    private float resumeNavigationTimer = 0f;

    // Tiempo de espera antes de reanudar la navegación después de agarrar el regalo
    public float resumeNavigationDelay = 2f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (target != null)
        {
            // Configurar la posición del objetivo del NavMeshAgent al personaje principal
            navMeshAgent.SetDestination(target.position);

            // Si está siguiendo al objeto, realizar el dash y lanzar el regalo
            if (isFollowingObject)
            {
                PerformDash();
                ThrowGift();
            }
        }

        if (resumeNavigationTimer > 0f)
        {
            // Contador de tiempo para reanudar la navegación después de agarrar el regalo
            resumeNavigationTimer -= Time.deltaTime;

            if (resumeNavigationTimer <= 0f)
            {
                // Tiempo de espera completo, reanuda la navegación
                ResumeNavigation();
            }
        }

        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            // El enemigo ha llegado al waypoint, avanza al siguiente
            SetDestination();
        }

        // Determinar si el agente se está moviendo
        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;

        // Rotar al enemigo en la dirección del movimiento
        if (isMoving)
        {
            RotateTowardsVelocity();
        }
    }
    
    void RotateTowardsVelocity()
    {
        // Obtener la dirección de la velocidad
        Vector3 velocityDirection = navMeshAgent.velocity.normalized;

        // Calcular la rotación hacia la dirección de la velocidad
        Quaternion toRotation = Quaternion.LookRotation(velocityDirection, Vector3.up);

        // Aplicar la rotación al enemigo
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 5f);
    }


    void PerformDash()
    {
        // Realizar el dash hacia el jugador
        Vector3 dashDirection = (target.position - transform.position).normalized;
        transform.position += dashDirection * dashSpeed * Time.deltaTime;
    }

    void ThrowGift()
    {
        // Lanzar el regalo hacia el jugador
        GameObject thrownGift = Instantiate(giftPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody giftRb = thrownGift.GetComponent<Rigidbody>();
        if (giftRb != null)
        {
            Vector3 throwDirection = (target.position - throwPoint.position).normalized;
            giftRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }

        // Reiniciar la siguiente iteración del dash y lanzamiento de regalo
        isFollowingObject = false;
    }

    void ResumeNavigation()
    {
        // Reanuda la navegación del NavMeshAgent
        navMeshAgent.isStopped = false;

        // Reanuda la ruta hacia el siguiente waypoint
        SetDestination();
    }

    void SetDestination()
    {
        if (target != null)
        {
            // Establecer el destino al personaje principal
            navMeshAgent.SetDestination(target.position);
        }
    }

    void OnDrawGizmos()
    {
        // Dibuja un gizmo esférico en el Editor para visualizar el radio de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Otros métodos y eventos...
}
