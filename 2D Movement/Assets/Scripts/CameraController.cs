using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Vector2 velocity;
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
            float playerDistance = Vector2.Distance(player1.transform.position, player2.transform.position);
            float camSize = Mathf.Clamp(playerDistance / 1.5f, 8, 15.5f);

            cam.orthographicSize = camSize;
        }
    }
}
