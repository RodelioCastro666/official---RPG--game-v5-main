using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private float xMin, xMax, yMin, yMax;
    //public float smoothing;
    //public Vector2 maxPosition;
    //public Vector2 minPosition;

    [SerializeField]
    private Tilemap tilemaps;

    private Player player;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = target.GetComponent<Player>(); 

        Vector3 minTile = tilemaps.CellToWorld(tilemaps.cellBounds.min);
        Vector3 maxTile = tilemaps.CellToWorld(tilemaps.cellBounds.max);

        SetLimits(minTile, maxTile);

        player.SetLimits(minTile, maxTile);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), 
           Mathf.Clamp(target.position.y, yMin, yMax), -10);

        //if (transform.position != target.position)
        //{
        //    Vector3 targetPosition = new(target.position.x, target.position.y, transform.position.z);

        //    targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        //    targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

        //    transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        //}

    }

    private void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float width =  height * cam.aspect;

        xMin = minTile.x + width / 2;
        xMax = maxTile.x - width / 2;

        yMin = minTile.y + height / 2;
        yMax = maxTile.y - height / 2;
    }

}
