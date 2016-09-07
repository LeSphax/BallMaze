using System.Collections;
using UnityEngine;

public delegate void MatrixBlendEnded();

[RequireComponent(typeof(Camera))]
public class MatrixBlender : MonoBehaviour
{
    private Camera m_Camera;

    void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }

    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration)
    {
        return LerpFromTo(m_Camera.projectionMatrix, dest, duration,null);
    }

    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration, MatrixBlendEnded endCallback)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            m_Camera.projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration);
            yield return 1;
        }
        m_Camera.projectionMatrix = dest;
        if (endCallback != null)
        {
            endCallback.Invoke();
        }
    }

    public Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration,MatrixBlendEnded endCallback =null)
    {
        StopAllCoroutines();
        return StartCoroutine(LerpFromTo(m_Camera.projectionMatrix, targetMatrix, duration, endCallback));
    }
}