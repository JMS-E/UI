using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    //public GameObject childRef;
    public Renderer rend;
    public float duration = 0.3f;

    public Color colorDefault = new Color(1.0f, 1.0f, 1.0f, 0.5f);//blue-ish
    public Color colorHighlighted = new Color(0.0f, 1.0f, 0.0f, 1.0f);//green-ish
    
    // Start is called before the first frame update
    void Start()
    {
        //childRef = this.gameObject.transform.GetChild(0).gameObject;
        rend = GetComponent<Renderer>();
        rend.material.SetColor("_TintColor", colorDefault); //
        //float lerp = Mathf.PingPong(Time.time, duration) / duration;
        //rend.material.color = Color.Lerp(colorDefault, colorHighlighted, lerp);
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerStay(Collider other)
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        rend.material.color = Color.Lerp(colorDefault, colorHighlighted, lerp);
    }
}
