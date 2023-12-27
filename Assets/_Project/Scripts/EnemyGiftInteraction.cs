using UnityEngine;

public class EnemyGiftInteraction : MonoBehaviour
{
    public Transform attachPoint; // Punto de anclaje para los regalos
    public LayerMask giftLayer;    // Capa que contiene los regalos
    private GameObject currentGift; // El regalo actualmente agarrado por el enemigo

    private void Update()
    {
        // Verificar si hay regalos en el área del enemigo
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, giftLayer);

        foreach (Collider collider in colliders)
        {
            // Verificar si el enemigo no tiene un regalo actualmente
            if (currentGift == null)
            {
                // Agarrar el regalo
                GrabGift(collider.gameObject);
            }
        }
    }

    private void GrabGift(GameObject gift)
    {
        // Desactivar la física del regalo
        Rigidbody giftRb = gift.GetComponent<Rigidbody>();
        if (giftRb != null)
        {
            giftRb.isKinematic = true;
            giftRb.useGravity = false;
        }

        // Anclar el regalo al punto de anclaje del enemigo
        gift.transform.SetParent(attachPoint);
        gift.transform.position = attachPoint.position;

        // Almacenar la referencia al regalo actual
        currentGift = gift;
    }
}