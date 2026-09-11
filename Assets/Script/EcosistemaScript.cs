using UnityEngine;
using System;
using System.Collections.Generic;

public class EcosistemaScript : MonoBehaviour
{
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
