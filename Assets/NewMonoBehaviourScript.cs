
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    bool printedLog = false;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private bool isStarted = false;
    
    void Start()
    {
        // Save the initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();

        // Ball stays in place until Start is pressed
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        gameObject.name = "Test 123";
        Debug.Log("Name changed to: " + gameObject.name);
    }

    void Update()
    {
        if (!isStarted)
            return;

        // Rotate the ball
        transform.Rotate(0, 1, 0);

        // Warning if the ball falls below -10
        if (transform.position.y < -10 && !printedLog)
        {
            Debug.LogWarning("Object has fallen below -10 on the Y-axis.");
            printedLog = true;
        }


        // Rainbow effect like Super Mario Star
        float hue = Mathf.Repeat(Time.time * 2f, 1f);
        GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 1f, 1f);
        

    }

    // Called when the Start button is pressed
    public void StartBall()
    {
        if (isStarted)
            return;

        isStarted = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    // Called when the Reset button is pressed
    public void ResetPosition()
    {
        isStarted = false;
        printedLog = false;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
