using System.Collections;
using UnityEngine;

public class SpawnGifts : MonoBehaviour
{
    [SerializeField]
    private GameObject[] giftPrefabs;  // Array de prefabs de regalos

    [SerializeField]
    private Transform spawnPoint;      // Punto donde se spawnearán los regalos

    [SerializeField]
    private float spawnInterval = 6f;   // Intervalo de tiempo entre spawneo de regalos

    [SerializeField]
    private float curveHeight = 2f;    // Altura de la curva

    [SerializeField]
    private float curveWidth = 2f;     // Ancho de la curva

    [SerializeField]
    private float forceMultiplier = .2f;  // Multiplicador de fuerza

    private void Start()
    {
        // Iniciar la corrutina para spawnear regalos cada 'spawnInterval' segundos
        StartCoroutine(SpawnGiftsPeriodically());
    }

    private IEnumerator SpawnGiftsPeriodically()
    {
        while (true)
        {
            // Spawnear un regalo
            SpawnGift();

            // Esperar el intervalo antes de spawnear el siguiente regalo
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnGift()
    {
        // Seleccionar aleatoriamente un tipo de regalo del array
        GameObject selectedGiftPrefab = giftPrefabs[Random.Range(0, giftPrefabs.Length)];

        // Instanciar el regalo en el spawnPoint
        GameObject newGift = Instantiate(selectedGiftPrefab, spawnPoint.position, Quaternion.identity);

        // Calcular la posición final
        Vector3 endPos = spawnPoint.position + spawnPoint.forward * curveWidth;

        // Aplicar una fuerza en forma de parábola
        Rigidbody rb = newGift.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 forceDirection = CalculateParabolicCurve(spawnPoint.position, endPos, curveHeight);
            float forceMagnitude = forceMultiplier * Mathf.Sqrt(curveWidth);  // Ajusta la magnitud de la fuerza según sea necesario
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
    }

    private Vector3 CalculateParabolicCurve(Vector3 start, Vector3 end, float height)
    {
        float distance = Vector3.Distance(start, end);
        float middlePointX = (start.x + end.x) / 2f;
        float middlePointY = (start.y + end.y) / 2f + height;
        Vector3 middle = new Vector3(middlePointX, middlePointY, start.z);

        AnimationCurve curve = new AnimationCurve(
            new Keyframe(0, start.y),
            new Keyframe(0.5f, middle.y),
            new Keyframe(1, end.y)
        );

        Vector3[] path = new Vector3[10];
        for (int i = 0; i < 10; i++)
        {
            float t = i / 9f;
            path[i] = new Vector3(
                Mathf.Lerp(start.x, end.x, t),
                curve.Evaluate(t),
                Mathf.Lerp(start.z, end.z, t)
            );
        }

        return path[1] - start;
    }
}
