using UnityEngine;
using UnityEngine.Tilemaps;

public class Control_Corona : MonoBehaviour
{
    [Header("Configuración Inicial")]
    public Tilemap tilemapTablero;

    [Header("Movimiento")]
    public float velocidad;
    public int distanciaMax;
    public float velocidadDeRotacion;
    private Vector3Int posicionActual;
    private Vector3 objetivo;

    bool enviado;
    public delegate void CambiarJugador();
    static public CambiarJugador cambiar;

    private void Awake()
    {
        tilemapTablero.CompressBounds();
    }

    void Start()
    {
        Control_Rey.comprobar += ComprobarAccion;

        Pocisionamiento();
    }

    private void OnDestroy()
    {
        Control_Rey.comprobar -= ComprobarAccion;
    }

    void Update()
    {
        Mover();
        transform.Rotate(0f, velocidadDeRotacion * Time.deltaTime, 0f);
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
        enviado = true;
    }

    void ComprobarAccion(GameObject rey)
    {
        if (rey.transform.position == transform.position)
        {

        }
        else
        {
            if (true)
                Calcularmovimiento();
        }
    }

    void Calcularmovimiento()
    {
        int distancia = Random.Range(-distanciaMax, distanciaMax + 1);
        int sentido = Random.Range(0, 2);

        CalcularObjetivo(distancia * sentido, distancia * (1 - sentido));
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

        enviado = false;
        objetivo = nuevaPos;
    }
    void Mover()
    {
        if (transform.position != objetivo)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);
        }
        else if (!enviado)
        {
            cambiar.Invoke();
            enviado = true;
        }
    }
}
