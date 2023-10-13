using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class SkillEffectsController : MonoBehaviour
{
    public List<ParticleSystem> particleEffectsInvoker = new List<ParticleSystem>();
    public List<VisualEffect> visualEffectsInvoker = new List<VisualEffect>();

    public List<ParticleSystem> particleEffectsAim = new List<ParticleSystem>();
    public List<VisualEffect> visualEffectsAim = new List<VisualEffect>();

    public GameObject shootSkillObject;

    public GameObject effectsParent;

    private Vector3 initialPosition;

    public float duration;


    public void PlayInvoker()
    {
        particleEffectsInvoker?.ForEach(x => x.Play());
        visualEffectsInvoker?.ForEach(x => x.Play());
    }


    public void PlayAimUnits(List<UnitFightController> aims)
    {
        foreach (var aim in aims)
        {
            effectsParent.transform.position = aim.transform.position;

            particleEffectsAim?.ForEach(x => x.Play());
            visualEffectsAim?.ForEach(x => x.Play());
        }
    }

    public void PlayAimHex(List<HexTile> hexes)
    {

        effectsParent.transform.position = hexes[0].transform.position;

        particleEffectsAim?.ForEach(x => x.Play());
        visualEffectsAim?.ForEach(x => x.Play());

    }

    public void StartCor(IEnumerator coroutine, int pause)
    {
        StartCoroutine(Cor(coroutine, pause));
    }

    public IEnumerator Cor(IEnumerator coroutine, int pause)
    {
        float time = 0;
        while (time < pause)
        {
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(coroutine);
    }

    public void StartCorShoot(IEnumerator coroutine, int pause, GameObject startPoint, List<UnitFightController> unitAims, List<HexTile> hexAims)
    {
        StartCoroutine(CorShoot(coroutine, pause, startPoint, unitAims, hexAims));
    }

    public IEnumerator CorShoot(IEnumerator coroutine, int pause, GameObject startPoint, List<UnitFightController> unitAims, List<HexTile> hexAims)
    {
        shootSkillObject.SetActive(true);
        var curPos = startPoint.transform.position;
        float time = 0;

        if (unitAims.Count > 0)
        {
            while (time < pause)
            {
                shootSkillObject.transform.position = Vector3.Lerp(curPos, unitAims[0].transform.position, time / pause);
                time += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (time < pause)
            {
                shootSkillObject.transform.position = Vector3.Lerp(curPos, hexAims[0].transform.position, time / pause);
                time += Time.deltaTime;
                yield return null;
            }
            PlayAimHex(hexAims);
        }

        shootSkillObject.SetActive(false);
        StartCoroutine(coroutine);

    }
}
