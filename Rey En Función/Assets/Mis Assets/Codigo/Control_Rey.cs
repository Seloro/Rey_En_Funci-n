using UnityEngine;
using UnityEngine.Tilemaps;

public class Control_Rey : MonoBehaviour
{
    [Header("Color")]
    public MeshRenderer mesh;
    public Color32 color;

    [Header("Configuración Inicial")]
    public bool ladoIzquierdo = true;
    public Tilemap tilemapTablero;
    public int columnasLimite = 5;

    [Header("Movimiento")]
    public float velocidad;
    private Vector3Int posicionActual;
    private Vector3 objetivo;

    bool enviado;
    public delegate void CambiarJugador();
    static public CambiarJugador cambiar;

    void Start()
    {
        mesh.material.color = color;
        Pocisionamiento();
    }

    private void Update()
    {
        Mover();
    }

    void Pocisionamiento()
    {
        BoundsInt limites = tilemapTablero.cellBounds;

        int columnaInicial = ladoIzquierdo
            ? Random.Range(limites.xMin + 1, limites.xMin + columnasLimite)
            : Random.Range(limites.xMax - columnasLimite, limites.xMax - 1);

        int filaInicial = Random.Range(limites.yMin, limites.yMax - 1);

        posicionActual = new Vector3Int(columnaInicial, filaInicial, 0);
        transform.position = tilemapTablero.GetCellCenterWorld(posicionActual) + Vector3.up;
        objetivo = transform.position;
    }

    public void IndicarMovimientoX(int x)
    {
        CalcularObjetivo(x, 0);
    }

    public void IndicarMovimientoY(int y)
    {
        CalcularObjetivo(0, y);
    }

    void CalcularObjetivo(int pasosX, int pasosY)
    {
        Vector3 direccion = new Vector3Int(pasosX, 0, pasosY);

        int cantidadPasos = Mathf.Max(Mathf.Abs(pasosX), Mathf.Abs(pasosY));

        Vector3 nuevaPos = transform.position;

        for (int i = 0; i < cantidadPasos; i++)
        {
            if (!Physics.Raycast(nuevaPos, direccion.normalized, 1f))
            {
                Vector3 siguiente = nuevaPos + direccion.normalized;

                if (Physics.Raycast(siguiente, Vector3.down, 1f))
                    nuevaPos = siguiente;
                else
                    i += cantidadPasos;
            }
            else
                i += cantidadPasos;
        }

        objetivo = nuevaPos;
        enviado = false;
    }

    void Mover()
    {
        if (transform.position != objetivo)
        {
            transform.LookAt(objetivo);

            transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);
        }
        else if (!enviado)
        {
            cambiar.Invoke();
            enviado = true;
        }
    }
}
