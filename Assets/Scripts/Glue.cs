using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class Glue : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    [SerializeField] private Transform glueTip;
    [SerializeField] private GameObject glueBlob;
    [SerializeField] private LayerMask glueableLayer; // Layer for objects that can be glued

    private bool triggerPressed;
    private bool touchingGlueable;

    [SerializeField] float glueRate = 0.1f;
    float nextGlueTime;


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
        triggerPressed = true;
        Debug.Log("Aplicando Cola");
    }

    private void OnDeactivate(DeactivateEventArgs args)
    {
        triggerPressed = false;
        Debug.Log("Cola fechada");
    }

    private void Update()
    {
        if (triggerPressed && touchingGlueable && Time.time > nextGlueTime)
        {
            nextGlueTime = Time.time + glueRate;
            ApplyGlue();
        }
    }

    void ApplyGlue()
    {
        RaycastHit hit;
        if (Physics.Raycast(glueTip.position, glueTip.forward, out hit, 0.1f, glueableLayer))
        {
            Debug.Log("Cola aplicada em: " + hit.collider.gameObject.name);
            GameObject blob = Instantiate(glueBlob, hit.point, Quaternion.LookRotation(hit.normal));
            blob.transform.SetParent(hit.transform); // Parent the glue blob to the hit object

            Rigidbody rb = blob.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Make the glue blob kinematic to prevent physics interactions
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & glueableLayer) != 0)
        {
            touchingGlueable = true;
        }
    }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & glueableLayer) != 0)
            {
                touchingGlueable = false;
            }
    }
}
