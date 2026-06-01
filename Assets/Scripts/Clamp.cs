using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Clamp : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    [SerializeField] private Transform clampPoint; // The point where the clamp will be attached
    [SerializeField] private Collider clampCollider; // The collider that will be used for the clamp

    private Transform grabedBug = null;
    private Transform selectedBug = null;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.activated.AddListener(OnActivate);
        grabInteractable.deactivated.AddListener(OnDeactivate);
    }

    private void OnDisable()
    {
        grabInteractable.activated.RemoveListener(OnActivate);
        grabInteractable.deactivated.RemoveListener(OnDeactivate);
    }

    private void OnActivate(ActivateEventArgs args)
    {
        Debug.Log("Pinça fechada");
        TakeBug();
        
    }

    private void OnDeactivate(DeactivateEventArgs args)
    { 
        Debug.Log("Pinça aberta");
        ReleaseBug();

    }

    private void TakeBug()
    {
        if (selectedBug == null || grabedBug != null) return;
        grabedBug = selectedBug;
        // Attach the object to the clamp point
        grabedBug.position = clampPoint.position;
        grabedBug.rotation = clampPoint.rotation;
        grabedBug.SetParent(clampPoint);

        Rigidbody rb = grabedBug.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Make the object kinematic to prevent physics interactions
        }
        
        
    }

    private void ReleaseBug()
    {
        if (grabedBug != null)
        {
            grabedBug.SetParent(null); // Detach the object from the clamp point
            Rigidbody rb = grabedBug.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Make the object non-kinematic to allow physics interactions
            }
            grabedBug = null; // Clear the reference to the grabbed object
            selectedBug = null; // Clear the reference to the selected object
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrou: " + other.name +
          " Tag: " + other.tag);

        selectedBug = other.transform;

        if (other.CompareTag("Bug"))
        {
            selectedBug = other.transform;
            Debug.Log("Pegou");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Saiu: " + other.name +
          " Tag: " + other.tag);
        if (other.CompareTag("Bug"))
        {
            selectedBug = null;
        }
    }
}
