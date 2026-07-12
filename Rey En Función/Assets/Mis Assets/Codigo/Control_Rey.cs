using UnityEngine;
using UnityEngine.Tilemaps;

public class Control_Rey : MonoBehaviour
{
    [Header("Color")]
    public MeshRenderer mesh;
    public Color32 color;

    [Header("Movimiento")]
    public float velocidad;
    private Vector3Int posicionActual;
    private Vector3 objetivo;
    public LayerMask mask;
    public LayerMask noPintar;

    bool enviado;
    int pintadasDisponibles;

    public delegate void ComprobarCorona(GameObject rey);
    static public ComprobarCorona comprobar;

    void Start()
    {
        mesh.material.color = color;
        objetivo = transform.position;
        enviado = true;
    }

    private void Update()
    {
        Mover();
        Pintar();
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

                if (Physics.Raycast(siguiente, Vector3.down, 1f, mask))
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
            comprobar.Invoke(gameObject);
            enviado = true;
        }
    }

    void Pintar()
    {
        if (pintadasDisponibles > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + (transform.forward * .2f), Vector3.down, out hit, 1f, noPintar))
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
