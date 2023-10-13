using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DissolveExample
{
    public class DissolveChilds : MonoBehaviour
    {
        // Start is called before the first frame update
        List<Material> materials = new List<Material>();
        bool PingPong = false;
        public VisualEffect graph;

        void Start()
        {
            var renders = GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material = new Material(renders[i].material);
                renders[i].material.SetFloat("_Dissolve", 0);
                materials.Add(renders[i].material); 
            }
        }

        public void ResetAndStart()
        {
            SetValue(0);
            StartCoroutine("Dissolve");
            graph.Play();
        }

        // Update is called once per frame
        private IEnumerator Dissolve()
        {
            float value = 0;
            int waitTime = 5;
            float elapsedTime = 0;
            while (value<1)
            {
                value = Mathf.Lerp(0,1, (elapsedTime/waitTime)* (elapsedTime / waitTime));
                SetValue(value);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }


        public void SetValue(float value)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve", value);
            }
        }
    }
}