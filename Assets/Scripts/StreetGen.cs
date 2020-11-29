using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StreetGen : MonoBehaviour
{
    public GameObject forestParent;
    public GameObject cityParent;
    public int segmentsAmount;
    public GameObject[] segmentPrefabs;
    public GameObject[] forestSegmentPrefabs;
    public bool reverse;
    int next;
    
    private void Start()
    {
        Generate();
    }

    public void Generate() {
        Cleanup();

        for (int i = 0; i < segmentsAmount; i++)
        {
            int rs = Random.Range(0, segmentPrefabs.Length);
            GameObject bob = Instantiate(segmentPrefabs[rs], cityParent.transform.localPosition + new Vector3(0, 0, next), transform.rotation);
            bob.transform.parent = cityParent.transform;
            bob.transform.localPosition = new Vector3(0, 0, next);
            if (!reverse)
                next += 14;
            else
                next -= 14;
        }

        next = 0;
        for (int i = 0; i < 1; i++)
        {
            int rs = Random.Range(0, forestSegmentPrefabs.Length);
            GameObject bob = Instantiate(forestSegmentPrefabs[rs], forestParent.transform.localPosition + new Vector3(0, 0, next), transform.rotation);
            bob.transform.parent = forestParent.transform;
            bob.transform.localPosition = new Vector3(0, 0, next);
            if (!reverse)
                next += 4;
            else
                next -= 4;
        }
    }

    public void ActivateForest() {
        cityParent.SetActive(false);
        forestParent.SetActive(true);
    }

    private void Cleanup() {
        foreach (Transform child in forestParent.transform) {
            Destroy(child.gameObject);
        }

        foreach (Transform child in cityParent.transform) {
            Destroy(child.gameObject);
        }
    }
}
