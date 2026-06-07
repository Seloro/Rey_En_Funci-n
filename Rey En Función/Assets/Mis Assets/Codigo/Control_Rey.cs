using UnityEngine;
using UnityEngine.Tilemaps;

public class Control_Rey : MonoBehaviour
{
    [Header("Configuración Inicial")]
    public bool ladoIzquierdo = true; // true = izquierda, false = derecha
    public Tilemap tilemapTablero;
    public int columnasLimite = 5;

    [Header("Movimiento")]
    public int pasosPorMovimiento = 3; // cantidad de casillas a mover
    public LayerMask capaObjetos; // capa para detectar colisiones con otros objetos

    private Vector3Int posicionActual;

    void Start()
    {
        if (tilemapTablero == null)
        {
            tilemapTablero = GameObject.Find("Tilemap_Tablero").GetComponent<Tilemap>();
        }

        // Obtener límites del tilemap
        BoundsInt limites = tilemapTablero.cellBounds;

        // Seleccionar columna inicial
        int columnaInicial = ladoIzquierdo
            ? Random.Range(limites.xMin, limites.xMin + columnasLimite)
            : Random.Range(limites.xMax - columnasLimite, limites.xMax);

        int filaInicial = Random.Range(limites.yMin, limites.yMax);

        posicionActual = new Vector3Int(columnaInicial, filaInicial, 0);
        transform.position = tilemapTablero.GetCellCenterWorld(posicionActual) + Vector3.up;
    }

    // Método público para los botones
    public void MoverRey(bool moverEnX)
    {
        Vector3Int direccion = moverEnX ? Vector3Int.right : Vector3Int.up;

        for (int i = 0; i < pasosPorMovimiento; i++)
        {
            Vector3Int nuevaPos = posicionActual + direccion;

            // Verificar si está dentro del tablero
            if (!tilemapTablero.HasTile(nuevaPos))
            {
                Debug.Log("Borde alcanzado, movimiento detenido.");
                break;
            }

            // Verificar si hay otro objeto
            Vector3 centroCasilla = tilemapTablero.GetCellCenterWorld(nuevaPos);
            Collider[] colisiones = Physics.OverlapSphere(centroCasilla, 0.3f, capaObjetos);
            if (colisiones.Length > 0)
            {
                Debug.Log("Casilla ocupada, movimiento detenido.");
                break;
            }

            // Mover
            posicionActual = nuevaPos;
            transform.position = centroCasilla + Vector3.up;
        }
    }
}
