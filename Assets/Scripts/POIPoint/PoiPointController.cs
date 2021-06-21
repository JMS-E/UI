using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PoiPointController : MonoBehaviour
{
    [Header("fill this With the point of interest Images.")]
    [GUIColor(0.6f, 1f, 0.6f, 1f)]
    public Sprite[] poiPhotoList;

    [Header("(de)activate the billboarding rotation for the close up image")]
    [GUIColor(0.6f, 1f, 0.6f, 1f)]
    public bool BigPictureBillboarding = false;
    
    [FoldoutGroup("Prefab setup attributes")]
    [InfoBox("do not change unless you have a good reason, you risk breaking this prefab", InfoMessageType.None)]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("this holds the references to the image buttons.")]
    [Required]
    public Button[] poiButtonList;

    [FoldoutGroup("Prefab setup attributes")]
    [InfoBox("do not change unless you have a good reason, you risk breaking this prefab",InfoMessageType.None)]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("this holds the reference to the activation buton.")]
    [Required]
    public Button poiActivationButton;

    [FoldoutGroup("Prefab setup attributes")]
    [InfoBox("do not change unless you have a good reason, you risk breaking this prefab",InfoMessageType.None)]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("this holds the reference to the zoomed in picture.")]
    [Required]
    public Button poiBigPictureButton;

    [FoldoutGroup("Prefab setup attributes")]
    [InfoBox("do not change unless you have a good reason, you risk breaking this prefab, this reference will set itself to the first camera tagged MainCamera",InfoMessageType.None)]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("this holds the reference to the players camera.")]
    public Camera poiCamera;

    [FoldoutGroup("Prefab setup attributes")]
    [InfoBox("do not change unless you have a good reason, you risk breaking this prefab",InfoMessageType.None)]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("this holds the reference to the PoiCanvas.")]
    [Required]
    public Canvas poiCanvas;

    [FoldoutGroup("Prefab setup attributes")]
    [InfoBox("do not change unless you have a good reason, you risk breaking this prefab",InfoMessageType.None)]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("this holds the reference to the billboarding script.")]
    [Required]
    public PoiPointBillboardHorizontalOnly poiPointBillboardHorizontalOnly;

    private bool _poiButtonsAreActive;
    private bool _BIgButtonsIsActive;
    private bool _poiActivationButtonIsActive;

    void Start()
    {
        poiCamera = Camera.main; //TODO set canvas.rect.pos.Y height to MainCamera.transform.Position.Y
        HidePOIphotos();
        HideBigPhoto();
        ShowpoiActivationButton();
        if (BigPictureBillboarding == false)
        {
            poiPointBillboardHorizontalOnly.enabled = false;
        }
        else
        {
            poiPointBillboardHorizontalOnly.enabled = true;
        }
    }

    public void ShowPOIphotos()
    {
        for (int index = 0; index < poiButtonList.Length; index++)
        {
            poiButtonList[index].image.enabled = true;
            poiButtonList[index].GetComponent<Image>().sprite = poiPhotoList[index];
        }
        _poiButtonsAreActive = true;
    }

    public void ShowBigPhoto()
    {
        poiBigPictureButton.image.enabled = true;
        _BIgButtonsIsActive = true;
    }

    public void ShowpoiActivationButton()
    {
        poiActivationButton.image.enabled = true;
        _poiActivationButtonIsActive = true;
    }

    public void HidePOIphotos()
    {
        foreach (var item in poiButtonList)
        {
            item.image.enabled = false;
        }
        _poiButtonsAreActive = false;
    }
    public void HideBigPhoto()
    {
        poiBigPictureButton.image.enabled = false;
        _BIgButtonsIsActive = false;
    }

    public void HidepoiActivationButton()
    {
        poiActivationButton.image.enabled = false;
        _poiActivationButtonIsActive = false;
    }

    public void TogglePoiPhotos()
    {
        if (_poiButtonsAreActive == false)
        {
            ShowPOIphotos();
        }
        else
        {
            HidePOIphotos();
        }
    }

    public void ToggleBigPhotos()
    {
        if (_BIgButtonsIsActive == false)
        {
            ShowBigPhoto();
            HidePOIphotos();
            HidepoiActivationButton();
        }
        else
        {
            HideBigPhoto();
            ShowPOIphotos();
            ShowpoiActivationButton();
        }
    }

    public void TogglepoiActivationButton()
    {
        if (_poiActivationButtonIsActive == false)
        {
            ShowpoiActivationButton();
        }
        else
        {
            HidepoiActivationButton();
        }
    }

    public void ShowBigPicture(Sprite pictureToShowBig)
    {
        poiBigPictureButton.GetComponent<Image>().sprite = pictureToShowBig;
        ToggleBigPhotos();
    }
}
