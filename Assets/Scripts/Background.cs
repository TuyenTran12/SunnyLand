using UnityEngine;

public class Background : MonoBehaviour
{
    public float relativeMove = 0.3f;
    public Vector2 offset;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void LateUpdate()
    {
        Vector3 cameraDelta = Camera.main.transform.position - startPos;
        transform.position = startPos + new Vector3(cameraDelta.x * relativeMove + offset.x, cameraDelta.y * relativeMove + offset.y, 0);
    }
}