using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
