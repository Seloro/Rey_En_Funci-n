using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
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

    private void Awake()
    {
        CargarEcuaciones();
    }

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

            boton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = listaPistas[indice].ecuacion;
        }
    }

    public void DesactivarBotonesYCronometro()
    {
        foreach (Button boton in listaBotones)
            boton.interactable = false;
    }

    void CargarEcuaciones()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ecuaciones.txt");

        if (File.Exists(path))
        {
            string[] lineas = File.ReadAllLines(path);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                string[] partes = linea.Split(',');

                if (partes.Length == 3)
                {
                    Pistas p = new Pistas
                    {
                        ecuacion = partes[0].Trim(),
                        resultado = int.Parse(partes[1].Trim()),
                        moverX = bool.Parse(partes[2].Trim())
                    };

                    listaPistas.Add(p);
                }
            }
        }
    }
}

[System.Serializable]
public class Pistas
{
    public string ecuacion;
    public int resultado;
    public bool moverX;
}