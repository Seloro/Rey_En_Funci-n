using Unity.VisualScripting;
using UnityEngine;

public class Casillas : MonoBehaviour
{
    BoxCollider col;

    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (true)
        {
            CaptarJugador();
        }
    }

    void CaptarJugador()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 2))
        {
            gameObject.layer = hit.collider.gameObject.layer;

            Control_Rey rey = hit.collider.GetComponent<Control_Rey>();
            if (rey != null)
            {
                Color32 colorImpacto = rey.color;

                Renderer rend = GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.SetColor("_Color_Base", colorImpacto);
                }
            }

            col.size = new Vector3(1, 2, 1);
        }
    }
}
