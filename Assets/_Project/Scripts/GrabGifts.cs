using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabGifts : MonoBehaviour
{
   [SerializeField] private GameObject handPoint;
    [SerializeField] private GameObject pickObject = null;
    [SerializeField] private float throwSpeed;
    [SerializeField] private Animator animator;

    void Update()
    {
        if (pickObject != null)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReleaseObject();
            }

            if (Input.GetMouseButtonDown(0))
            {
                ThrowObject();
            }
        }
    }

    private void ThrowObject()
    {
        if (pickObject != null)
        {
            Rigidbody rb = pickObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    Vector3 direction = (hitInfo.point - transform.position).normalized;
                    animator.SetTrigger("Launch");
                    // Aplicar fuerza al objeto para lanzarlo en esa dirección
                    rb.AddForce(direction * throwSpeed, ForceMode.Impulse);
                }

                pickObject.gameObject.transform.SetParent(null);
                pickObject = null;
                
               
            }
        }
    }

    private void ReleaseObject()
    {
        if (pickObject != null)
        {
            pickObject.GetComponent<Rigidbody>().useGravity = true;
            pickObject.GetComponent<Rigidbody>().isKinematic = false;
            
            pickObject.gameObject.transform.SetParent(null);

            pickObject = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            if (Input.GetKeyDown(KeyCode.E) && pickObject == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;

                other.transform.position = handPoint.transform.position;
                other.gameObject.transform.SetParent(handPoint.gameObject.transform);

                pickObject = other.gameObject;
            }
        }
    }
}
