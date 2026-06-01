using UnityEngine;

public class GlueableArea : MonoBehaviour
{
    public ObjectPartsIdentificationSO objectPartsID;
    public bool hasGlue = false;

    private bool alreadyGlued = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyGlued) return;

        GlueableArea otherArea = other.GetComponent<GlueableArea>();

        if (otherArea == null || otherArea.alreadyGlued)
            return;

        bool matchingID =
            otherArea.objectPartsID.brokenConnectionID ==
            objectPartsID.brokenConnectionID;

        bool gluePresent =
            hasGlue || otherArea.hasGlue;

        if (matchingID && gluePresent)
        {
            // trava os dois
            alreadyGlued = true;
            otherArea.alreadyGlued = true;

            GlueTogether(otherArea);

            Debug.Log("Objects glued together!");
        }
    }

    void GlueTogether(GlueableArea otherArea)
    {
        Vector3 spawnPos =
            (transform.position +
             otherArea.transform.position) * 0.5f;

        Quaternion spawnRot = transform.rotation;

        Instantiate(
            objectPartsID.gluedResultPrefab,
            spawnPos,
            spawnRot
        );

        Destroy(transform.root.gameObject);
        Destroy(otherArea.transform.root.gameObject);

        Debug.Log("Novo objeto criado!");
    }
}