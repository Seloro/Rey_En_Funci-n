using System.Collections.Generic;
using UnityEngine;

public class Posicionador : MonoBehaviour
{
    [Header("Objetos a posicionar")]
    public List<Transform> jugadores;
    public Transform corona;
    public Transform contenedorPowerUP;
    List<Transform> powerUP = new List<Transform>();

    [Header("Contenedores de anclas")]
    public Transform contenedorPosicionesJugador;
    public Transform contenedorPosicionesCorona;
    public Transform contenedorPosicionesPowerUP;
    List<Transform> listaPosicionesJugador = new List<Transform>();
    List<Transform> listaPosicionesCorona = new List<Transform>();
    List<Transform> listaPosicionesPowerUP = new List<Transform>();

    [Header("Power UP")]
    public bool sinPowerUP;

    private void Awake()
    {
        CargarHijosEnLista(contenedorPosicionesJugador, listaPosicionesJugador);
        CargarHijosEnLista(contenedorPosicionesCorona, listaPosicionesCorona);

        AsignarPosicionAleatoria(corona, listaPosicionesCorona);
        AsignarPosicionAleatoria(jugadores, listaPosicionesJugador);

        if (!sinPowerUP)
        {
            CargarHijosEnLista(contenedorPosicionesPowerUP, listaPosicionesPowerUP);
            CargarHijosEnLista(contenedorPowerUP, powerUP);

            AsignarPosicionAleatoria(powerUP, listaPosicionesPowerUP);
        }
    }

    public void CargarHijosEnLista(Transform contenedor, List<Transform> lista)
    {
        lista.Clear();

        foreach (Transform hijo in contenedor)
            lista.Add(hijo);
    }

    public void AsignarPosicionAleatoria(Transform pieza, List<Transform> lista)
    {
        int indice = Random.Range(0, lista.Count);
        Transform seleccionado = lista[indice];

        pieza.position = seleccionado.position;

        lista.RemoveAt(indice);
    }

    public void AsignarPosicionAleatoria(List<Transform> piezas, List<Transform> lista)
    {
        foreach (Transform pieza in piezas)
        {
            int indice = Random.Range(0, lista.Count);
            Transform seleccionado = lista[indice];

            pieza.position = seleccionado.position;

            lista.RemoveAt(indice);
        }
    }
}
