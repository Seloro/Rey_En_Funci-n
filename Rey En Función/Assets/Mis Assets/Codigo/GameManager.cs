using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Jugadores")]
    public Control_Rey[] reyes;
    int jugador;

    [Header("Botones")]
    public GameObject contenedorBotones;
    private List<Button> listaBotones = new List<Button>();

    [Header("Pistas")]
    public List<Pistas> listaPistas = new List<Pistas>();
    List<int> indicesDisponibles = new List<int>();

    private void Start()
    {
        Control_Rey.cambiar += CambiarJugador;

        foreach (Transform hijo in contenedorBotones.transform)
            listaBotones.Add(hijo.GetComponent<Button>());

        SetearBotones();
    }

    private void OnDestroy()
    {
        Control_Rey.cambiar -= CambiarJugador;
    }

    void CambiarJugador()
    {
        jugador = 1 - jugador;
        SetearBotones();
    }

    void SetearBotones()
    {
        foreach (Button boton in listaBotones)
        {
            boton.interactable = true;

            if (indicesDisponibles.Count == 0)
                indicesDisponibles = Enumerable.Range(0, listaPistas.Count).ToList();

            int indiceAleatorio = Random.Range(0, indicesDisponibles.Count);
            int indice = indicesDisponibles[indiceAleatorio];
            indicesDisponibles.RemoveAt(indiceAleatorio);

            boton.onClick.RemoveAllListeners();
            boton.onClick.AddListener(() => DesactivarBotonesYCronometro());

            if (listaPistas[indice].moverX)
                boton.onClick.AddListener(() => reyes[jugador].IndicarMovimientoX(listaPistas[indice].resultado));
            else
                boton.onClick.AddListener(() => reyes[jugador].IndicarMovimientoY(listaPistas[indice].resultado));
        }
    }

    public void DesactivarBotonesYCronometro()
    {
        foreach (Button boton in listaBotones)
            boton.interactable = false;
    }
}

[System.Serializable]
public class Pistas
{
    public Sprite ecuacion;
    public int resultado;
    public bool moverX;
}