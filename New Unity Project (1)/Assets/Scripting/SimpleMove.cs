using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public Transform target;
    private Vector3 targetPos;

    private Vector3 randomTargetPos;
    public float randomTargetDistMin = 2;
    public float randomTargetDistMax = 10;
    public bool useRandomTarget = false;

    public float distStop = 1;
    public float distSlowDown = 2;
    private float vitesse = 0;
    public float vitesseMax = 1.0f;
    private float vitesseMin = 0.1f;
    public float acceleration = 1.0f;

    private bool atDestination = false;

    public bool drawLineTarget = true;


    void Start()
    {
        SetRandomTargetPos();
    }

    void SetRandomTargetPos()
    {
        randomTargetPos = Random.onUnitSphere;
        randomTargetPos.y = 0;
        randomTargetPos = randomTargetPos.normalized * Random.Range(randomTargetDistMin, randomTargetDistMax);
    }

    void Update()
    {
        //Debug (après le calcul de targetPos)
        if (drawLineTarget)
            Debug.DrawLine(transform.position, targetPos, GetComponent<Renderer>().material.color);
        
        //Mise a jour du comportement
        if (target == null)
            useRandomTarget = true;

        //Calcul du point de destination
        if (useRandomTarget || target == null)
            targetPos = randomTargetPos;
        else
            targetPos = target.position;

        //Distance au point
        Vector3 deplacement = targetPos - transform.position;
        float distance = deplacement.magnitude;
        float distanceRestante = distance - distStop;
        atDestination = distanceRestante <= 0;

        //Déplacement
        if (!atDestination)
        {
            float vitesseVoulue = vitesseMax;
            if (distanceRestante < distSlowDown - distStop)
                vitesseVoulue = Mathf.Lerp(vitesseMax, vitesseMin, 1.0f - (distanceRestante / (distSlowDown - distStop)));

            //Prise en compte de l'accélération
            if (vitesseVoulue > vitesse)
                vitesse = Mathf.Min(vitesse + acceleration * Time.deltaTime, vitesseVoulue);
            else
                vitesse = vitesseVoulue; //on freine parfaitement bien

            //Déplacement
            deplacement = deplacement.normalized * vitesse * Time.deltaTime;
            transform.position += deplacement;
        }
        else
        {
            SetRandomTargetPos();
        }
    }

 public bool drawGizmoTarget = true;

    void OnDrawGizmosSelected()

    {
        if (drawGizmoTarget) 
        {
            // Sphère rouge a la distance de stop
            Gizmos.color = new Color(1, 0, 0, 1.0f);
            Gizmos.DrawWireSphere(targetPos, distStop);
            // Sphère bleue a la distance de freinage
            Gizmos.color = new Color(0, 0, 1, 1.0f);
            Gizmos.DrawWireSphere(targetPos, distSlowDown);
        }
    }
}