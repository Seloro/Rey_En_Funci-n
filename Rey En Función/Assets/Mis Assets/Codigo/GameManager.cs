using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.AppUI.Redux;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Jugadores")]
    public Control_Rey[] reyes;
    public Color[] colores;
    int jugador;

    [Header("Botones")]
    public GameObject contenedorBotones;
    private List<Button> listaBotones = new List<Button>();

    [Header("Pistas")]
    public List<Pistas> listaPistas = new List<Pistas>();
    List<int> indicesDisponibles = new List<int>();

    [Header("Temporización")]
    public List<Coronas> imagenes = new List<Coronas>();
    public float tiempoMaximo;
    public float[] temp;
    bool restar;

    [Header("Comprobacion de movimiento")]
    public List<PistasEjes> listaPistasEjes = new List<PistasEjes>();
    List<Pistas> listaComprobacion = new List<Pistas>();
    bool[] ejes = new bool[4];
    int respuestasCorrectas;

    private void Awake()
    {
        CargarEcuaciones();
    }

    private void Start()
    {
        Control_Corona.cambiar += CambiarJugador;

        foreach (Transform hijo in contenedorBotones.transform)
            listaBotones.Add(hijo.GetComponent<Button>());

        jugador = Random.Range(0, 2);
        SetearBotones();
    }

    private void OnDestroy()
    {
        Control_Corona.cambiar -= CambiarJugador;
    }

    private void Update()
    {
        Temporizador();
    }

    void CambiarJugador()
    {
        jugador = 1 - jugador;
        Invoke("SetearBotones", 1);
    }

    void SetearBotones()
    {
        restar = true;

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

            listaComprobacion.Add(listaPistas[indice]);

            ComprobadorDeOpcionesBalidas();

            ColorBlock color = boton.colors;
            color.highlightedColor = colores[jugador];
            color.pressedColor = new Vector4(colores[jugador].r * .5f, colores[jugador].g * .5f, colores[jugador].b * .5f, 1);
            boton.colors = color;
        }
    }

    void ComprobadorDeOpcionesBalidas()
    {
        respuestasCorrectas = 0;

        for (int i = 0; i < ejes.Length; i++)
            ejes[i] = false;

        if (!Physics.Raycast(reyes[jugador].gameObject.transform.position, Vector3.right, 1f))
        {
            Vector3 siguiente = reyes[jugador].gameObject.transform.position + Vector3.right;

            if (Physics.Raycast(siguiente, Vector3.down, 1f, ~(1 << reyes[jugador].gameObject.layer)))
                ejes[0] = true;
        }
        else if (!Physics.Raycast(reyes[jugador].gameObject.transform.position, Vector3.left, 1f))
        {
            Vector3 siguiente = reyes[jugador].gameObject.transform.position + Vector3.left;

            if (Physics.Raycast(siguiente, Vector3.down, 1f, ~(1 << reyes[jugador].gameObject.layer)))
                ejes[1] = true;
        }
        else if (!Physics.Raycast(reyes[jugador].gameObject.transform.position, Vector3.forward, 1f))
        {
            Vector3 siguiente = reyes[jugador].gameObject.transform.position + Vector3.forward;

            if (Physics.Raycast(siguiente, Vector3.down, 1f, ~(1 << reyes[jugador].gameObject.layer)))
                ejes[2] = true;
        }
        else if (!Physics.Raycast(reyes[jugador].gameObject.transform.position, Vector3.back, 1f))
        {
            Vector3 siguiente = reyes[jugador].gameObject.transform.position + Vector3.back;

            if (Physics.Raycast(siguiente, Vector3.down, 1f, ~(1 << reyes[jugador].gameObject.layer)))
                ejes[3] = true;
        }

        foreach (Pistas pista in listaComprobacion)
        {
            if (pista.moverX && pista.resultado > 0 && ejes[0] == true)
                respuestasCorrectas++;
            else if (pista.moverX && pista.resultado < 0 && ejes[1] == true)
                respuestasCorrectas++;
            else if (!pista.moverX && pista.resultado > 0 && ejes[2] == true)
                respuestasCorrectas++;
            else if (!pista.moverX && pista.resultado < 0 && ejes[3] == true)
                respuestasCorrectas++;
        }

        if (respuestasCorrectas <= 0)
        {
            int pocicion = 0;

            for (int i = 0; i < ejes.Length; i++)
            {
                if (ejes[i])
                {
                    listaBotones[pocicion].onClick.RemoveAllListeners();
                    listaBotones[pocicion].onClick.AddListener(() => DesactivarBotonesYCronometro());

                    int indiceEje = Random.Range(0, listaPistasEjes[i].ejes.Count);

                    if (listaPistasEjes[i].ejes[indiceEje].moverX)
                        listaBotones[pocicion].onClick.AddListener(() => reyes[jugador].IndicarMovimientoX(listaPistasEjes[i].ejes[indiceEje].resultado));
                    else
                        listaBotones[pocicion].onClick.AddListener(() => reyes[jugador].IndicarMovimientoY(listaPistasEjes[i].ejes[indiceEje].resultado));

                    listaBotones[pocicion].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = listaPistasEjes[i].ejes[indiceEje].ecuacion;

                    pocicion++;
                }
            }

            MescladorDeBotones();
        }
    }

    void MescladorDeBotones()
    {
        System.Random rng = new System.Random();
        int n = listaBotones.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Button value = listaBotones[k];
            listaBotones[k] = listaBotones[n];
            listaBotones[n] = value;
        }
    }

    public void DesactivarBotonesYCronometro()
    {
        foreach (Button boton in listaBotones)
            boton.interactable = false;

        restar = false;
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

    void Temporizador()
    {
        if (restar)
        {
            temp[jugador] -= Time.deltaTime;

            imagenes[jugador].color.fillAmount = Mathf.Clamp01(temp[jugador] / tiempoMaximo);

            imagenes[jugador].color.color = new Vector4(imagenes[jugador].color.color.r, imagenes[jugador].color.color.g, imagenes[jugador].color.color.b, 1);
            imagenes[jugador].negro.color = new Vector4(imagenes[jugador].negro.color.r, imagenes[jugador].negro.color.g, imagenes[jugador].negro.color.b, 1);
            imagenes[1 - jugador].color.color = new Vector4(imagenes[1 - jugador].color.color.r, imagenes[1 - jugador].color.color.g, imagenes[1 - jugador].color.color.b, .5f);
            imagenes[1 - jugador].negro.color = new Vector4(imagenes[1 - jugador].negro.color.r, imagenes[1 - jugador].negro.color.g, imagenes[1 - jugador].negro.color.b, .5f);
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

[System.Serializable]
public class PistasEjes
{
    public List<Pistas> ejes = new List<Pistas>();

}

[System.Serializable]
public class Coronas
{
    public Image color;
    public Image negro;
}