using System;
using UnityEngine;
using UnityEngine.Events;

public class SphereLauncher : MonoBehaviour
{
    public GameObject spherePrefab;
    public float launchForce = 10f;
    public float launchHeight = 5f;

    // Evento que se activará cuando la esfera golpee a una galleta de jengibre
    public UnityEvent OnSphereHitGingerbread;

    void Update()
    {
        // Lanzar esferas con clic derecho
        if (Input.GetMouseButtonDown(1))
        {
            LaunchSphere();
        }
    }
    
        void LaunchSphere()
        {
            // Instanciar la esfera
            GameObject sphere = Instantiate(spherePrefab, transform.position, Quaternion.identity);

            // Obtener la rotación del personaje y calcular la dirección de lanzamiento
            Vector3 launchDirection = transform.forward;

            // Elevar la esfera
            launchDirection.y = launchHeight;

            // Obtener el componente Rigidbody de la esfera y aplicar la fuerza en la dirección calculada
            Rigidbody sphereRb = sphere.GetComponent<Rigidbody>();
            sphereRb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);

            // Destruir la esfera después de 3 segundos
            Destroy(sphere, 3f);
        }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision with: " + other.gameObject.name);

        if (other.gameObject.CompareTag("gingerbread"))
        {
            // Llama al método Hit() del script Gingerbread
            Gingerbread gingerbreadScript = other.gameObject.GetComponent<Gingerbread>();
            if (gingerbreadScript != null)
            {
                gingerbreadScript.Hit();
            }
        }
    }

}