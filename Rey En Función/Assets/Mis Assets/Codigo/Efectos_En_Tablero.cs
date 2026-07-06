using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Efectos_En_Tablero : MonoBehaviour
{
    [Header("Comprobacion de movimiento")]
    public Tilemap tablero;
    public bool[] efectos;
    public int cantidadDeCasillasAfectadas;
    public LayerMask capas;

    private void Start()
    {
        Enviar_A_Tablero.enviar += ActivarEfecto;
        GameManager.verificar += AplicarEfectos;
    }

    private void OnDestroy()
    {
        Enviar_A_Tablero.enviar -= ActivarEfecto;
        GameManager.verificar -= AplicarEfectos;
    }

    void AplicarEfectos()
    {
        if (efectos[0])
        {
            efectos[0] = false;
            DesactivarAleatorias();
        }
        else if (efectos[1])
        {
            efectos[1] = false;
            MoverAleatorias();
        }
    }
    void ActivarEfecto(int valor)
    {
        efectos[valor] = true;
    }

    void DesactivarAleatorias()
    {
        BoundsInt bounds = tablero.cellBounds;
        int desactivadas = 0;

        while (desactivadas < cantidadDeCasillasAfectadas)
        {
            int x = Random.Range(bounds.xMin, bounds.xMax);
            int y = Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int celda = new Vector3Int(x, y, 0);

            if (!tablero.HasTile(celda)) continue;

            Vector3 centro = tablero.GetCellCenterWorld(celda);

            RaycastHit hit;
            if (!Physics.Raycast(centro, Vector3.up, out hit, 1f, capas))
            {
                tablero.SetTile(celda, null);
                desactivadas++;
            }
        }
    }

    void MoverAleatorias()
    {
        BoundsInt bounds = tablero.cellBounds;
        List<Vector3Int> casillasLibres = new List<Vector3Int>();
        List<Vector3Int> casillasOcupadas = new List<Vector3Int>();

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tablero.HasTile(pos))
            {
                Vector3 centro = tablero.GetCellCenterWorld(pos);
                if (!Physics.Raycast(centro, Vector3.up, 1f, capas))
                {
                    casillasOcupadas.Add(pos);
                }
            }
            else
            {
                Vector3 centro = tablero.GetCellCenterWorld(pos);
                if (!Physics.Raycast(centro, Vector3.up, 1f, capas))
                {
                    casillasLibres.Add(pos);
                }
            }
        }

        int movidas = 0;
        while (movidas < cantidadDeCasillasAfectadas && casillasOcupadas.Count > 0 && casillasLibres.Count > 0)
        {
            Vector3Int origen = casillasOcupadas[Random.Range(0, casillasOcupadas.Count)];
            Vector3Int destino = casillasLibres[Random.Range(0, casillasLibres.Count)];

            TileBase tile = tablero.GetTile(origen);

            tablero.SetTile(origen, null);
            tablero.SetTile(destino, tile);

            casillasOcupadas.Remove(origen);
            casillasLibres.Remove(destino);

            movidas++;
        }

    }
}
