using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public List<GameObject> players;
    private List<Vector2> playersPos;

    private Vector2 velocity;
    private Camera cam;
    private float sizeVelocity;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        players = new List<GameObject>();
        playersPos = new List<Vector2>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (players.Count > 0)
        {
            // Camera Position
            playersPos.Clear();
            foreach (GameObject obj in players)
            {
                playersPos.Add(obj.transform.position);
            }

            Vector2 targetPos = playersPos.Aggregate(new Vector2(0, 0), (s, v) => s + v) / (float)playersPos.Count; ;
            Vector2 Pos2D = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, 0.25f);
            transform.position = new Vector3(Pos2D.x, Pos2D.y, transform.position.z);

            // Camera Scale
            List<float> playerX = new List<float>();
            foreach (Vector2 obj in playersPos)
            {
                playerX.Add(obj.x);
            }
            List<float> playerY = new List<float>();
            foreach (Vector2 obj in playersPos)
            {
                playerY.Add(obj.y);
            }

            float playerDistance = Mathf.Max(Mathf.Max(playerX.ToArray()) - Mathf.Min(playerX.ToArray()), (Mathf.Max(playerY.ToArray()) - Mathf.Min(playerY.ToArray())) * cam.aspect);
            float camSize = Mathf.Clamp(playerDistance / 2.4f, 7f, 18f);

            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, camSize, ref sizeVelocity, 0.4f);
        }
    }

    public void removeFromCam(GameObject toRemove)
    {
        players.Remove(toRemove);
    }

    public void addToCam(GameObject addition)
    {
        players.Add(addition);
    }
}
