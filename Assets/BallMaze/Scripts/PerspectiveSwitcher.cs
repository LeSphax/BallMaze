using UnityEngine;

public delegate void PerspectiveSwitched();

[RequireComponent(typeof(MatrixBlender))]
public class PerspectiveSwitcher : MonoBehaviour
{
    private const float animationTime = 0.5f;
    private new Camera camera;

    private Matrix4x4 ortho,
                        perspective;
    public float fov = 60f,
                        near = .3f,
                        far = 1000f,
                        orthographicSize = 5f;
    private float aspect;
    private MatrixBlender blender;
    private static bool orthoOn;

    public MatrixBlendEnded currentCallback;

    void Awake()
    {
        blender = GetComponent<MatrixBlender>();
    }

    void Start()
    {
        camera = GetComponent<Camera>();

        aspect = ((float)Screen.width / Screen.height);

        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
        camera.projectionMatrix = ortho;
        orthoOn = true;
        
    }

    public void SetOrthographic(bool value, MatrixBlendEnded endCallback)
    {
        orthoOn = value;
        currentCallback = endCallback;
        Blend();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            orthoOn = !orthoOn;
            Blend();
        }
    }

    private void Blend()
    {
        if (camera != null)
        {
            if (orthoOn)
                blender.BlendToMatrix(ortho, animationTime, currentCallback);
            else
                blender.BlendToMatrix(perspective, animationTime, currentCallback);
        }
        else
        {
            Invoke("Blend", 0.1f);
        }
    }
}
