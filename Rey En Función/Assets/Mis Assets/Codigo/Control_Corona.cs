using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Control_Corona : MonoBehaviour
{
    [Header("Configuración Inicial")]
    public Color32 color;

    [Header("Movimiento")]
    public float velocidad;
    public int distanciaMax;
    public float velocidadDeRotacion;
    private Vector3 objetivo;
    public LayerMask mask;

    bool enviado;
    int pintadasDisponibles;

    public delegate void CambiarJugador();
    static public CambiarJugador cambiar;

    void Start()
    {
        Control_Rey.comprobar += ComprobarAccion;

        objetivo = transform.position;
        enviado = true;
    }

    private void OnDestroy()
    {
        Control_Rey.comprobar -= ComprobarAccion;
    }

    void Update()
    {
        Mover();
        Pintar();
        transform.Rotate(0f, velocidadDeRotacion * Time.deltaTime, 0f);
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

                if (Physics.Raycast(siguiente, Vector3.down, 1f, mask))
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

    void Pintar()
    {
        if (pintadasDisponibles > 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, mask))
            {
                pintadasDisponibles--;

                hit.collider.gameObject.layer = gameObject.layer;

                Renderer rend = hit.collider.gameObject.GetComponent<Renderer>();
                if (rend != null)
                    rend.material.SetColor("_Color_Base", color);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pintador"))
            pintadasDisponibles += 10;
    }
}
