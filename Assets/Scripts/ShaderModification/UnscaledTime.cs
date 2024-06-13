using UnityEngine;

public class UnscaledTime : MonoBehaviour
{
    public Material targetMaterial;
    private float realTime;

    private void Update()
    {
        realTime += Time.unscaledDeltaTime;

        if (targetMaterial != null)
        {
            targetMaterial.SetFloat("_RealTime", realTime);
        }
    }
}
