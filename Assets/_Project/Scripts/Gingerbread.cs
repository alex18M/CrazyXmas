using UnityEngine;
using UnityEngine.Events;

public class Gingerbread : MonoBehaviour
{
    private GingerbreadSpawner spawner;
    private int hitCount = 0;
    public int maxHits = 2;
    public ParticleSystem smokeHit;

    public UnityEvent OnGingerbreadDestroyed;

    public void SetSpawner(GingerbreadSpawner gingerbreadSpawner)
    {
        spawner = gingerbreadSpawner;
    }

    // Llamado cuando la galleta de jengibre es destruida
    public void DestroyGingerbread()
    {
        // Notificar al spawner que la galleta de jengibre ha sido destruida
        if (spawner != null)
        {
            spawner.GingerbreadDestroyed();
        }

        // Aquí puedes realizar cualquier otra lógica necesaria al destruir la galleta de jengibre
        // Por ejemplo, reproducir efectos de partículas, sonidos, etc.

        // Llamar al evento de destrucción
        OnGingerbreadDestroyed.Invoke();
        smokeHit.gameObject.SetActive(true);

        // Finalmente, destruir el objeto de la galleta de jengibre
        Destroy(gameObject);
    }

    // Este método puede ser llamado cuando la galleta de jengibre es golpeada
    public void Hit()
    {
        // Incrementar el contador de golpes
        hitCount++;
        Debug.Log("Gingerbread hit!");
        // Llamar al evento de golpe si está conectado
        OnHit();

        // Verificar si la galleta de jengibre ha alcanzado el número máximo de golpes
        if (hitCount >= maxHits)
        {
            // Destruir la galleta de jengibre si ha alcanzado el número máximo de golpes
            DestroyGingerbread();
        }
    }

    // Evento para notificar cuando la galleta de jengibre es golpeada
    private void OnHit()
    {
        // Puedes agregar lógica adicional aquí si es necesario
    }
}