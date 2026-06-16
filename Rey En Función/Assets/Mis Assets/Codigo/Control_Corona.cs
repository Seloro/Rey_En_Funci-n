using UnityEngine;
using UnityEngine.Tilemaps;

public class Control_Corona : MonoBehaviour
{
    [Header("Configuración Inicial")]
    public Tilemap tilemapTablero;

    [Header("Movimiento")]
    public float velocidad;
    private Vector3Int posicionActual;
    private Vector3 objetivo;

    private void Awake()
    {
        tilemapTablero.CompressBounds();
    }

    void Start()
    {
        Pocisionamiento();
    }

    void Update()
    {
        
    }

    void Pocisionamiento()
    {
        BoundsInt limites = tilemapTablero.cellBounds;

        int centroX = (limites.xMin + limites.xMax) / 2;
        int centroY = (limites.yMin + limites.yMax) / 2;

        Vector3Int[] casillasCentrales = new Vector3Int[]
        {
        new Vector3Int(centroX,     centroY,     0),
        new Vector3Int(centroX - 1, centroY,     0),
        new Vector3Int(centroX,     centroY - 1, 0),
        new Vector3Int(centroX - 1, centroY - 1, 0)
        };

        int indice = Random.Range(0, casillasCentrales.Length);
        posicionActual = casillasCentrales[indice];

        transform.position = tilemapTablero.GetCellCenterWorld(posicionActual) + Vector3.up;
        objetivo = transform.position;
    }
}
