using Engine.Core;

namespace Engine.GLApi
{
    public interface IRenderable
    {
        void Render(Camera cam, eRenderMode mode = eRenderMode.shaded);
        Shader Shader { get; set; }
        bool EnableCull { get; set; }
        OpenTK.Vector4 Color { get; set; }
        bool ShowBoundingBox { get; set; }
    }
}
