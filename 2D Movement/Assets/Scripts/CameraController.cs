using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Vector2 velocity;
    private float sizeVelocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (player1 && player2)
        {
            // Camera Position
            Vector2 targetPos = Vector2.Lerp(player1.transform.position, player2.transform.position, 0.5f);

            Vector2 Pos2D = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, 0.25f);
            transform.position = new Vector3(Pos2D.x, Pos2D.y, transform.position.z);

            // Camera Scale
            float playerDistance = Mathf.Max(Mathf.Abs(player1.transform.position.x - player2.transform.position.x), Mathf.Abs(player1.transform.position.y - player2.transform.position.y) * cam.aspect);
            float camSize = Mathf.Clamp(playerDistance / 2.4f, 7f, 18f);

            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, camSize, ref sizeVelocity, 0.4f);
        }
    }
}
