using CustomAnimations.BallMazeAnimations;
using UnityEngine;

public class TileView : GameObjectWithMesh
{
    private Renderer meshRenderer;


    public override GameObject Mesh
    {
        get
        {
            return base.Mesh;
        }

        set
        {
            base.Mesh = value;
            meshRenderer = value.GetComponent<Renderer>();
        }
    }

    public Color Color
    {
        get
        {
            return meshRenderer.material.color;
        }
        set
        {
            meshRenderer.material.color = value;
        }
    }

    internal ColorAnimation GetFillingAnimation(float duration)
    {
        return ColorAnimation.CreateColorAnimation(Mesh, Color.yellow, duration, 1);
    }
}
