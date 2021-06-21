using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class haptichoovesTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   // public MeshRenderer receivingRenderer;


#if UNITY_EDITOR
    private void Reset()
    {
       // receivingRenderer = GetComponentInChildren<MeshRenderer>();
    }
#endif

    public void OnEnable()
    {

    }

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("ENTER");

    }

    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log("EXIT");

    }

}