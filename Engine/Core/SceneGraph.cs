using Engine.GLApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public class SceneGraph 
    {
        public List<IRenderable> SceneItems;

        public SceneGraph()
        {
            SceneItems = new List<IRenderable>();
        }

        public void Clean()
        {
            SceneItems.Clear();
        }

        public void AddObject(IRenderable renderable)
        {
            SceneItems.Add(renderable);
        }

        public void RenderAll(Camera cam)
        {
            foreach (var item in SceneItems)
            {
                SetDirectLight(item, cam);
                item.Render(cam, eRenderMode.shaded);
            }
        }
        
        public void SetDirectLight(IRenderable obj, Camera cam)
        {
            obj.Shader.Use();
            obj.Shader.SetVec3("cameraPosition", cam.Position);
            obj.Shader.SetVec3("directLight.direction", -0.2f, -1.0f, 0.0f);
            obj.Shader.SetVec3("directLight.ambient", 0.25f, 0.25f, 0.25f);
            obj.Shader.SetVec3("directLight.diffuse", 0.5f, 0.5f, 0.4f);
            obj.Shader.SetVec3("directLight.specular", 0.1f, 0.1f, 0.1f);
            obj.Shader.SetFloat("material.shineness", 2.0f);
        }

    }

}
