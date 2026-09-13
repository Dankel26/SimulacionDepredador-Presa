using UnityEngine; // esta directiva da acceso a las funciones del motor de Unity como Vector3, MonoBehaviour, GameObject, RandomTranform, Quaternion y demas
using System;
using System.Collections.Generic;// este hace funcionar los objetos List<GameObject>

public class EcosistemaScript : MonoBehaviour
{
    // Declaramos las variables

    [Header("Simulacion")] // los Heardes lo que hacen es que se muestren en el Inspector para modificarse desde ahi
    public int initialConejos = 10; // valores iniciales
    public int initialZorros = 5;
    public int actualConejos; // valores que se guardaran en el transcurso de la simulacion, se puede reiniciar la simulacion sin perder la config original
    public int actualZorros;
    public int cazaPerDay = 2;
    public int nacerConejoPerDay = 5;

    [Header("Tiempo")]
    public float secondsPerDay = 1f; // este determina cuantos segundos reales dura cada dia en la simualcion
    private int day = 0; // el contador de dias, se va acomulando cada que se reinicia el timer
    private float timer = 0; // este es un contador que se rige del secondsPerDay, cada que llegue a 1f se reinicia y cada vez que reinicia va pasando los dias

    [Header("Visual")]
    public GameObject conejoPrefab; // este solo es una referencia de los prefab en Unity, se asignan en el Inspector
    public GameObject zorroPrefab;
    public Transform sabanaArea;// este toma en cuenta su pocision y escala para definir el area donde se reproduciran los objetos, por eso es un Transform
    private List<GameObject> conejoObjects = new List<GameObject>(); // estas listas guardan los gameobjects que se muestran actualmente para luego eliminarlos, asi evita acumular elementos fantasmas.
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
                // el Random.Range genera un numero flotante aleatorio
                UnityEngine.Random.Range(-sabanaArea.localScale.x / 2, sabanaArea.localScale.x / 2),
                UnityEngine.Random.Range(-sabanaArea.localScale.y / 2, sabanaArea.localScale.y / 2),
                0
            ); 

            Vector3 worldPos = sabanaArea.position + randomPos; // calculamos la posicion del GameObject en global tomando en cuenta la posicion local del area de la sabana, es decir, sumamos ambos para crear una posicion nueva y aleatoria para un conejo

            // creamos una copia del prefab conejo, para renderizarla en pantalla
            GameObject conejo = Instantiate(conejoPrefab, worldPos, Quaternion.identity); // el Quaterion.identity significa que se crea la copia sin rotar o va estar en rotacion neutra
            conejoObjects.Add(conejo); // guardamos la copia en la lista

        } // Y ASI PARA LOS ZORROS

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
            Destroy(conejo); //este metodo heredado de MonoBehaviour, define cada GameObject como destruido, ejemplo, lo manda a la papelera de reciclaje de Win
        }
        foreach (var zorro in zorroObjects)
        {
            Destroy(zorro);
        }
        conejoObjects.Clear(); // esto vacia la lista de objetos, ejemplo, vacia la papelera de reciclaje de Win
        zorroObjects.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // aqui hace que pase el tiempo por segundos y no por frame

        if (timer >= secondsPerDay) // cuando se acomula el tiempo necesario en seg, ya paso un dia
        {
            timer = 0; // reinicia el contador para volver hacer el conteo del deltaTime
            SimulationDay();
        }

        void SimulationDay()
        {
            if (actualConejos <= 0 || actualZorros <= 0) return; // si ambas poblaciones llega a 0, se corta la ejecucion y regresa, NO HACE NADA, SOLO TERMINA LA SIMULACION

            // ------------------------------------------------------------------------------------------------------------------------------------------//
            // NOTA
            // el RETURN, corta la ejecucion de la funcion actual, en este caso la funcion local SimulationDay(), si pusieramos todo este codigo dentro del if en el metodo Update(), el RETURN corta ESE metodo Update(), por lo que, no se aumentaria los dias de la simulacion, lo que haria que no se actualizara ningun cambio.

            // ------------------------------------------------------------------------------------------------------------------------------------------//

            day++; // si no, pasamos al siguiente dia

            // reproduccion de los conejos
            // int nacimiento = actualConejos + nacerConejoPerDay;
            
            int cazas = actualZorros * cazaPerDay; // calculamos las posibles cazas que pueden hacer los zorros 

            if (cazas > actualConejos) cazas = actualConejos; // si las posibles cazas superan el numero de la poblacion, entonces reemplazamos esa tasa por el numero de conejos que hay
            actualConejos -= cazas; // reducimos esas cazas en la poblacion actual de conejos, es decir, actualizamos la poblacion

            if (day % 3 == 0 && actualZorros > 0) // por cada 3 dias que pasan, un zorro muere, se hace calculando el residuo de la division
            {
                actualZorros--;
                Debug.Log("Zorro muerto");
            }

            ClearSabana();// limpiamos la pantalla de los GameObject anteriores
            DrawSabana(); // sea actualiza la pantalla con los nuevos GameObject

            //Porbamos
            Debug.Log("Dia " + day + ": " + actualConejos + " conejos y " + actualZorros + " zorros");

            // estos if no aparecen si la condicion del RETURN se cumple :v
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
