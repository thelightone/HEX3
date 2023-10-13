using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speedTime = 2;
    public float highCurve = 2;
    public ParticleSystem explos;
    public GameObject startPoint;
    public HexTile destTile;
  
    public void FlyToEnemy(HexTile tile)
    {
        destTile = tile;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        transform.position = startPoint.transform.position;
        transform.rotation = startPoint.transform.rotation;
        StartCoroutine(IFlyToEnemy());
    }

    private IEnumerator IFlyToEnemy()
    {
        float elapsedTime = 0;
        var curPosition = transform.position;
        var curRotation = transform.rotation;
        Vector3 curRotationEuler = transform.rotation.eulerAngles;
        var targPosition = new Vector3(destTile.transform.position.x, 1, destTile.transform.position.z);
        var targRotation = Quaternion.Euler(90+(90-curRotationEuler.x), curRotationEuler.y, curRotationEuler.z);

        var curvePos = new Vector3(Vector3.Lerp(curPosition, targPosition, 0.5f).x, highCurve, Vector3.Lerp(curPosition, targPosition, 0.5f).z);

        while (elapsedTime < speedTime)
        {
            transform.position = Bezier(curPosition, curvePos, targPosition, SmoothStep(elapsedTime / speedTime));
            transform.rotation = Quaternion.Lerp(curRotation, targRotation, elapsedTime / speedTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        destTile.unitOn.fightController.ReceiveDamage(true);
        explos.Play();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

    }

    private Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float progress)
    {
        var ab = Vector3.Lerp(a, b, progress);
        var bc = Vector3.Lerp(b, c, progress);
        var abc = Vector3.Lerp(ab, bc, progress);
        return abc;
    }

    private float SmoothStep(float progress)
    {
        var progressCub = 5 * (progress - 0.5f) * (progress - 0.5f) * (progress - 0.5f) + 0.5f;
        progress = Mathf.Lerp(progress, progressCub, (highCurve/10-0.1f));
        return progress;
    }
   
}
