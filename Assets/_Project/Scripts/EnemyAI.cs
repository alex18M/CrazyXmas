using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;  // Puntos a los que el enemigo se dirigirá
    public float detectionRadius = 5f;  // Radio de detección para encontrar regalos
    private int currentWaypoint = 0;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isFollowingObject = false;
    private Transform targetObject;
    private float resumeNavigationTimer = 0f;
    private float timeSinceLastGrab = 0f;
    private GameObject currentGiftObject = null;



    // Tiempo de espera antes de reanudar la navegación después de agarrar el regalo
    public float resumeNavigationDelay = 2f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Asegúrate de que el componente NavMeshAgent esté presente y activo
        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
        {
            SetDestination();
        }
        else
        {
            Debug.LogError("NavMeshAgent not present or not active on enemy: " + name);
        }
    }

    void SetDestination()
    {
        if (waypoints.Length > 0)
        {
            // Establecer el destino al waypoint actual
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        }
        else
        {
            Debug.LogError("No waypoints assigned to enemy: " + name);
        }
    }

    void Update()
    {
        if (!isFollowingObject)
        {
            // Solo intenta buscar objetos si no está siguiendo actualmente un objeto
            FindObjectsInRadius();
        }
        else
        {
            // Está siguiendo un objeto, maneja esa lógica
            HandleFollowingObject();
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
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            SetDestination();
        }

        // Determinar si el agente se está moviendo
        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;

        // Activar la animación correspondiente
        animator.SetBool("WALK", isMoving);
        animator.SetBool("IDLE", !isMoving);

        // Rotar al enemigo en la dirección del movimiento
        if (isMoving)
        {
            RotateTowardsVelocity();
        }
        
        // Incrementa el tiempo transcurrido desde el último agarre
        timeSinceLastGrab += Time.deltaTime;

        // Verifica si ha pasado el tiempo necesario para lanzar el regalo
        if (timeSinceLastGrab >= 5f && currentGiftObject != null)
        {
            // Lanza el regalo con mucha fuerza
            Rigidbody giftRigidbody = currentGiftObject.GetComponent<Rigidbody>();
            if (giftRigidbody != null)
            {
                float launchForce = 20f;  // Ajusta la fuerza según sea necesario
                giftRigidbody.AddForce(transform.forward * launchForce, ForceMode.Impulse);
            }

            // Limpia el regalo actual
            currentGiftObject = null;
        }
    }

    void OnDrawGizmos()
    {
        // Dibuja un gizmo esférico en el Editor para visualizar el radio de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void FindObjectsInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Object"))
            {
                // Encontró un regalo, comienza a seguirlo
                StartFollowingObject(collider.transform);
                break;  // Solo sigue al primer regalo que encuentre
            }
        }
    }

    void StartFollowingObject(Transform objectTransform)
    {
        // Detiene la navegación actual del NavMeshAgent
        navMeshAgent.isStopped = true;

        // Almacena el objeto que se está siguiendo
        targetObject = objectTransform;

        // Indica que el enemigo está siguiendo un objeto
        isFollowingObject = true;
    }

    void HandleFollowingObject()
    {
        Debug.Log("Handling Following Object");

        // Verifica si el objeto aún existe
        if (targetObject != null)
        {
            // Debug.Log("Target Object Position: " + targetObject.position);
            // Debug.Log("Enemy Position: " + transform.position);
            //
            // // Verificar el estado del NavMeshAgent
            // Debug.Log("NavMeshAgent isStopped: " + navMeshAgent.isStopped);

            // Verificar la distancia
            float remainingDistance = navMeshAgent.remainingDistance;
            float stoppingDistance = navMeshAgent.stoppingDistance;
            // Debug.Log("Remaining Distance: " + remainingDistance);
            // Debug.Log("Stopping Distance: " + stoppingDistance);
            
            // Actualiza el destino del NavMeshAgent para seguir al objeto
            navMeshAgent.SetDestination(targetObject.position);

            // Verifica si ha llegado al objeto (la distancia restante es menor que la distancia de detención)
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                // Asegúrate de que el objeto objetivo sea válido
                if (targetObject != null)
                {
                    // Ejecuta la lógica para agarrar el regalo
                    GrabGift(targetObject.gameObject);

                    // Configura el temporizador de espera antes de reanudar la navegación
                    resumeNavigationTimer = resumeNavigationDelay;

                    // Almacena el regalo actual y reinicia el temporizador para lanzarlo después de 5 segundos
                    currentGiftObject = targetObject.gameObject;
                    timeSinceLastGrab = 0f;

                    // Limpia el objeto objetivo
                    targetObject = null;

                    // Reanuda la navegación del NavMeshAgent
                    navMeshAgent.isStopped = false;

                    // Reanuda la ruta hacia el siguiente waypoint
                    SetDestination();

                    // Indica que el enemigo ya no está siguiendo un objeto
                    isFollowingObject = false;
                }
            }
        }
        else
        {
            // Si el objeto ya no existe, reanuda la navegación
            ResumeNavigation();
        }
    }

    void ResumeNavigation()
    {
        // Limpia el objeto objetivo
        targetObject = null;

        // Restablece la rotación del enemigo a la rotación original
        transform.rotation = Quaternion.identity;

        // Reanuda la navegación del NavMeshAgent
        navMeshAgent.isStopped = false;

        // Reanuda la ruta hacia el siguiente waypoint
        SetDestination();

        // Indica que el enemigo ya no está siguiendo un objeto
        isFollowingObject = false;
    }


    void GrabGift(GameObject giftObject)
    {
        // Implementa la lógica para agarrar el regalo
        // En el ejemplo, el regalo se destruye, pero puedes personalizar esto según tus necesidades
        Destroy(giftObject);
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
    
}
