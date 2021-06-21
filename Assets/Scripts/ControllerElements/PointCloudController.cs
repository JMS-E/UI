using BAPointCloudRenderer.CloudController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloudController : MonoBehaviour
{
    public bool HideAtStart = true;
    public List<GameObject> gameObjectsToHide;

    private DynamicPointCloudSet dynamicPointCloudSet;
    private bool isShowingPointCloud = true;

    void Start()
    {
        dynamicPointCloudSet = this.GetComponent<DynamicPointCloudSet>();

        // Run a coroutine to wait till PointRenderer is initiated
        if (HideAtStart)
            StartCoroutine(HidePointCloudAtStart());
    }

    public void TogglePointCloud()
    {
        if (isShowingPointCloud)
        {
            HidePointCloud();
        }
        else
        {
            ShowPointCloud();
        }
    }

    public void ShowPointCloud()
    {
        if(!isShowingPointCloud)
        {
            dynamicPointCloudSet.PointRenderer.Display();

            foreach (GameObject gj in gameObjectsToHide)
            {
                gj.SetActive(false);
            }

            isShowingPointCloud = true;
        }
    }

    public void HidePointCloud()
    {
        if (isShowingPointCloud)
        {
            dynamicPointCloudSet.PointRenderer.Hide();

            foreach (GameObject gj in gameObjectsToHide)
            {
                gj.SetActive(true);
            }

            isShowingPointCloud = false;
        }
    }

    private IEnumerator HidePointCloudAtStart()
    {
        // Wait till PointRenderer is Initiated
        while (dynamicPointCloudSet.PointRenderer == null)
        {
            yield return new WaitForSeconds(.1f);
        }

        HidePointCloud();

        yield return null;
    }
}
