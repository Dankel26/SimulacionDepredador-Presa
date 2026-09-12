using UnityEngine;
using System;
using System.Collections.Generic;

public class EcosistemaScript : MonoBehaviour
{
    // Declaramos las variables

    [Header("Simulacion")]
    public int initialConejos = 10;
    public int initialZorros = 5;
    public int actualConejos;
    public int actualZorros;
    public int cazaPerDay = 2;
    public int nacerConejoPerDay = 5;

    [Header("Tiempo")]
    public float secondsPerDay = 1f;
    private int day = 0;
    private float timer = 0;

    [Header("Visual")]
    public GameObject conejoPrefab;
    public GameObject zorroPrefab;
    public Transform sabanaArea;
    private List<GameObject> conejoObjects = new List<GameObject>();
    private List<GameObject> zorroObjects = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Guardamos las variables iniciales en las actuales
        actualConejos = initialConejos;
        actualZorros = initialZorros;
        //
        DrawSabana();
        // Probamos si funciona con Debug
        Debug.Log("Dia " + day + ": " + actualConejos + " conejos y " + actualZorros + " zorros");
    }

    void DrawSabana() //--> ya esta en privado sin necesidad de ponerlo
    {
        // renderizamos los conejos en pantalla
        for (int i = 0; i < actualConejos; i++)
        {
            // se define con un vector de 3 porque en Unity se toma en cuenta las 3 dimensiones
            // calculamos la posicion de un objeto de manera aleatoria en el area de la sabana
            Vector3 randomPos = new Vector3(
                // dividimos en dos para que no se salga del area
                UnityEngine.Random.Range(-sabanaArea.localScale.x / 2, sabanaArea.localScale.x / 2),
                UnityEngine.Random.Range(-sabanaArea.localScale.y / 2, sabanaArea.localScale.y / 2),
                0
            ); 

            Vector3 worldPos = sabanaArea.position + randomPos;
            // definimos el objeto a la posicion calculada
            GameObject conejo = Instantiate(conejoPrefab, worldPos, Quaternion.identity);
            conejoObjects.Add(conejo);

        }

        // renderizamos los zorros en pantalla
        for (int i = 0; i < actualZorros; i++)
        {
            // se define con un vector de 3 porque en Unity se toma en cuenta las 3 dimensiones
            // calculamos la posicion de un objeto de manera aleatoria en el area de la sabana
            Vector3 randomPos = new Vector3(
                // dividimos en dos para que no se salga del area
                UnityEngine.Random.Range(-sabanaArea.localScale.x / 2, sabanaArea.localScale.x / 2),
                UnityEngine.Random.Range(-sabanaArea.localScale.y / 2, sabanaArea.localScale.y / 2),
                0
            ); 

            Vector3 worldPos = sabanaArea.position + randomPos;
            // definimos el objeto a la posicion calculada
            GameObject zorro = Instantiate(zorroPrefab, worldPos, Quaternion.identity);
            zorroObjects.Add(zorro);

        }
    }

    void ClearSabana()
    {
        foreach (var conejo in conejoObjects)
        {
            Destroy(conejo);
        }
        foreach (var zorro in zorroObjects)
        {
            Destroy(zorro);
        }
        conejoObjects.Clear();
        zorroObjects.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= secondsPerDay)
        {
            timer = 0;
            SimulationDay();
        }

        void SimulationDay()
        {
            if (actualConejos <= 0 || actualZorros <= 0) return;
            day++;

            // reproduccion de los conejos
            // int nacimiento = actualConejos + nacerConejoPerDay;
            
            // conejos cazados por los zorros
            int cazas = actualZorros * cazaPerDay;

            if (cazas > actualConejos) cazas = actualConejos;
            actualConejos -= cazas;

            if (day % 3 == 0 && actualZorros > 0)
            {
                actualZorros--;
                Debug.Log("Zorro muerto");
            }

            ClearSabana();
            DrawSabana();

            //Porbamos
            Debug.Log("Dia " + day + ": " + actualConejos + " conejos y " + actualZorros + " zorros");

            if (actualConejos <= 0)
            {
                Debug.Log("Los conejos han muerto");
            }
            else if (actualZorros <= 0)
            {
                Debug.Log("Los conejos han sobrevivido de los zorros");
                
            }
        }

    }
}
