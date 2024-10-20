using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    // Item hiện thông tin
    [Header("Detected Parameters")]

    public Transform detectionPoint;
    private const float detectionRadius = 0.2f;
    public LayerMask detectionLayer;
    public GameObject detectedObject;

    [Header("Examine Fields")]

    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public bool isExamining = false;

    [Header("Others")]

    public List<GameObject> pickedItems = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isExamining) return;

        if (DetectObject() && InteractInput())
        {
            detectedObject.GetComponent<Item>().Interact();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.F);
    }
    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        //if(obj == null)
        //{
        //    detectedObject = null;
        //    return false;
        //}
        //else
        //{
        //    detectedObject = obj.gameObject;
        //    return true;
        //}

        detectedObject = obj?.gameObject;
        return obj != null;
    }
    public void ExamineItem(Item item)
    {
        examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
        examineText.text = item.descriptionText;
        examineWindow.SetActive(true);
        isExamining = true;
    }
    public void CloseExamineWindow()
    {
        examineWindow.SetActive(false);
        isExamining = false;
    }
}
