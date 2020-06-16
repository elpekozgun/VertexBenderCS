using Engine.Core;
using System.Windows.Forms;

namespace VertexBenderCS.Forms
{
    public class SceneNode : TreeNode
    {
        public Transform Transform;

        public SceneNode(Transform transform)
        {
            Transform = transform;
            base.Text = transform.Name;
            base.Name = transform.Name;
        }
    }

}
