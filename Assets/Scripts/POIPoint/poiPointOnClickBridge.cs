using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class poiPointOnClickBridge : MonoBehaviour
{
    // Start is called before the first frame update
    [FoldoutGroup("Prefab setup attributes")]
    [InfoBox("do not change unless you have a good reason, you risk breaking this prefab")]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("this holds the reference to the PoiPointController.")]
    public GameObject poiPointController;
    [Required]

    private Sprite poiSpriterite;

    public void ClickBridge()
    {
        //return poiSpriterite = GetComponent<Image>().sprite;
        poiSpriterite = GetComponent<Image>().sprite;
        poiPointController.GetComponent<PoiPointController>().ShowBigPicture(poiSpriterite);
    }
}
