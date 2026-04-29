using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;

    public GameObject player;

    private float yUpperBound;
    private float yLowerBound;
    private float xUpperBound;
    private float xLowerBound;

    private float cameraSize;


    void Start()
    {
        cameraSize = GetComponent<Camera>().orthographicSize;

        yUpperBound = top.transform.position.y - cameraSize;
        yLowerBound = bottom.transform.position.y + cameraSize;
        xUpperBound = right.transform.position.x - cameraSize * 2;
        xLowerBound = left.transform.position.x + cameraSize * 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2) player.transform.position;
        if (transform.position.x > xUpperBound)
        {
            transform.position = new Vector2(xUpperBound, transform.position.y);
        }
        if (transform.position.x < xLowerBound)
        {
            transform.position = new Vector2(xLowerBound, transform.position.y);
        }
        if (transform.position.y > yUpperBound)
        {
            transform.position = new Vector2(transform.position.x, yUpperBound);
        }
        if (transform.position.y < yLowerBound)
        {
            transform.position = new Vector2(transform.position.x, yLowerBound);
        }
        transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
    }
}
