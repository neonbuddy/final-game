using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    private Transform target;               //Camera follows player

    private float xMax, xMin, yMin, yMax;

    /// <summary>
    /// A reference to the ground tilemap
    /// </summary>
    [SerializeField]
    private Tilemap tilemap;

    private Player player;  

    // Start is called before the first frame update
    void Start()
    {
        //Creates a reference to the target
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Creates a reference to the player's script so SetLimits can be called
        player = target.GetComponent<Player>();

        //Calculates the min and max postion (Based on world bounds)
        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

        //Sets the limits of the camera
        SetLimits(minTile, maxTile);

        //Sets the limits of the player
        player.SetLimits(minTile, maxTile);
    }

    // When player is done moving, camera will follow
    public void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="minTile"></param>
    /// <param name="maxTile"></param>
    private void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        Camera cam = Camera.main;       //Reference to main camera

        float height = 2f * cam.orthographicSize;   //Orthographic camera used in 2D (2*Orthosize)
        float width = height * cam.aspect;          //calc width

        xMin = minTile.x + width / 2;
        xMax = maxTile.x - width / 2;

        yMin = minTile.y + height / 2;
        yMax = maxTile.y - height / 2;

    }

}
