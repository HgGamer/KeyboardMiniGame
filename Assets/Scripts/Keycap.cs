using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Keycap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public bool fixedbutton = false;
    float snapdistance = 50;
    public string label;
    public string labelBR;
    public string labelBL;
    public string labelTR;
    public string labelTL;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        //canvasGroup = GetComponent<CanvasGroup>();
        //if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (label == "")
        {
            transform.GetChild(1).GetComponent<TMP_Text>().fontSize = 40;
            transform.GetChild(2).GetComponent<TMP_Text>().fontSize = 40;
            transform.GetChild(3).GetComponent<TMP_Text>().fontSize = 40;
            transform.GetChild(4).GetComponent<TMP_Text>().fontSize = 40;

        }
        if (label.Length > 1)
        {
            transform.GetChild(0).GetComponent<TMP_Text>().fontSize = 34;

        }
        transform.GetChild(0).GetComponent<TMP_Text>().text = label;
        transform.GetChild(1).GetComponent<TMP_Text>().text = labelBR;
        transform.GetChild(2).GetComponent<TMP_Text>().text = labelBL;
        transform.GetChild(3).GetComponent<TMP_Text>().text = labelTR;
        transform.GetChild(4).GetComponent<TMP_Text>().text = labelTL;
        canvas = GetComponentInParent<Canvas>();

        rectTransform.localScale = rectTransform.localScale * canvas.scaleFactor;
        if(fixedbutton){
            this.enabled = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //canvasGroup.blocksRaycasts = false;
        transform.localEulerAngles=Vector3.zero;
        if (transform.parent.GetComponent<Switch>())
        {
            transform.SetParent( transform.parent.parent,true);
                
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // canvasGroup.blocksRaycasts = true;
        // Check for snapping
        SnapToTarget();
    }

    void SnapToTarget()
    {
        Switch[] switches = FindObjectsOfType<Switch>();

        foreach (var sw in switches)
        {
            if (Vector3.Distance(sw.transform.position, transform.position) < snapdistance* canvas.scaleFactor && sw.transform.childCount == 0)
            {
                transform.position = sw.transform.position;
                transform.SetParent(sw.transform,true);
                return;
            }

        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
