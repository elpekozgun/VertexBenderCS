namespace Engine.Core
{
    public enum eComponent
    {
        meshRenderer,
        lineRenderer,
        light,
        camera
    }


    public interface IComponent
    {
        eComponent ComponentType { get; }
        IComponent GetComponent();
    }
}
