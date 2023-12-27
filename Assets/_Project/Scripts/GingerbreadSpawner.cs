using System.Collections;
using UnityEngine;

public class GingerbreadSpawner : MonoBehaviour
{
    public GameObject gingerbreadPrefab;
    public Transform spawnPoint;
    public float respawnTime = 5f;
    public int maxHits = 2;

    private int currentHits;

    void Start()
    {
        SpawnGingerbread();
    }

    void SpawnGingerbread()
    {
        GameObject newGingerbread = Instantiate(gingerbreadPrefab, spawnPoint.position, Quaternion.identity);
        Gingerbread gingerbreadComponent = newGingerbread.GetComponent<Gingerbread>();

        if (gingerbreadComponent != null)
        {
            gingerbreadComponent.SetSpawner(this);
        }
        else
        {
            Debug.LogError("Gingerbread component not found on the spawned object.");
        }
    }

    public void GingerbreadDestroyed()
    {
        currentHits = 0;
        StartCoroutine(RespawnAfterDelay());
    }

    public void GingerbreadHit()
    {
        currentHits++;

        if (currentHits >= maxHits)
        {
            GingerbreadDestroyed();
        }
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnGingerbread();
    }
}