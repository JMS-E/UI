
using UnityEngine;

public class ToggleGameobject : MonoBehaviour
{
    public void toggleThisGameobject()
    {

        gameObject.SetActive(!gameObject.activeSelf);
    }
}
