using UnityEngine;

public class ObjectFloater : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public float wobbleSpeed = 2f;
    public float height = 0;
    public float amplitude = 0.1f;

    private Vector3 initialPosition;
    private Vector3 tempPos;
    private float yPos;
    private bool highlighted;
    private float elapsedTime;

    private void OnMouseEnter ()
    {
        highlighted = true;
        elapsedTime = 0;
    }

    private void OnMouseExit ()
    {
        highlighted = false;
    }

    private void Awake ()
    {
        initialPosition = transform.position;
        tempPos = transform.position;
        yPos = tempPos.y;
    }

    private void Update ()
    {
        if (highlighted)
        {
            elapsedTime += Time.deltaTime;
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            tempPos.y = yPos + height + amplitude * Mathf.Sin(wobbleSpeed * elapsedTime);
            transform.position = tempPos;
        }
        else
        {
            transform.position = initialPosition;
        }
    }
}
