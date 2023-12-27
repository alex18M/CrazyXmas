using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GiftGiving : MonoBehaviour
{
    public GiftPropertiesSO[] giftPropertiesArray;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            // Obtener el GiftObject del objeto que colisiona
            GiftObject giftObject = other.GetComponent<GiftObject>();

            if (giftObject != null && giftObject.giftProperties != null && giftObject.giftProperties.IsGift && !giftObject.hasCollided)
            {
                foreach (var giftProperties in giftPropertiesArray)
                {
                    if (giftProperties != null && giftProperties.IsGift && giftProperties.type == giftObject.giftProperties.type)
                    {
                        // Realizar acciones basadas en el tipo de regalo
                        PlayerController.Instance.AddPoints(giftProperties.points);

                        // Desactivar el objeto físico del regalo
                        //giftObject.gameObject.SetActive(false);

                        // Other actions you may want to perform when the player enters the trigger with a gift

                        // You can add more specific logic for each type of gift if needed
                    }
                }
            }
        }
    }
}
