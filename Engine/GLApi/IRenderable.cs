using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GLApi
{
    public interface IRenderable
    {
        void Render(eRenderMode mode);
        Shader Shader { get; set; }
    }
}
