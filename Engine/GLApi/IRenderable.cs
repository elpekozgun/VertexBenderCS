using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GLApi
{
    public interface IRenderable
    {
        void Render(Camera cam, eRenderMode mode = eRenderMode.shaded);
        Shader Shader { get; set; }
        Transform Transform {get;set;}
    }
}
