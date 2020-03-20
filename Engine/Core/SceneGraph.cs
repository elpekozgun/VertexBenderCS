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
        public List<Transform> SceneItems;
        public Action<Transform> OnItemAdded;
        public Action<Transform> OnItemDeleted;
        public Action OnSceneCleared;

        public bool IsBlinnPhong;

        public SceneGraph()
        {
            SceneItems = new List<Transform>();
            IsBlinnPhong = true;
        }

        public void Clean()
        {
            SceneItems.Clear();
            OnSceneCleared?.Invoke();
        }

        public void AddObject(Transform item)
        {
            SceneItems.Add(item);
            OnItemAdded?.Invoke(item);
        }

        public void DeleteObject(Transform item)
        {
            foreach (var child in item.Children)
            {
                DeleteObject(child);
            }
            SceneItems.Remove(item);
            OnItemDeleted?.Invoke(item);
        }

        public void RenderAll(Camera cam, eRenderMode renderMode)
        {
            foreach (var item in SceneItems)
            {
                if (item is IRenderable)
                {
                    var renderable = item as IRenderable;

                    SetDirectLight(renderable, cam);
                    renderable.Render(cam, renderMode);
                }
            }
        }
        
        public void SetDirectLight(IRenderable obj, Camera cam)
        {
            obj.Shader.Use();
            obj.Shader.SetVec3("cameraPosition", cam.Position);
            obj.Shader.SetVec3("directLight.direction", -0.1f, -1.0f, 0.1f);
            obj.Shader.SetVec3("directLight.ambient", 0.25f, 0.25f, 0.25f);
            obj.Shader.SetVec3("directLight.diffuse", 0.5f, 0.5f, 0.4f);
            obj.Shader.SetVec3("directLight.specular", 0.1f, 0.1f, 0.1f);
            obj.Shader.SetBool("IsBlinnPhong", IsBlinnPhong);
            obj.Shader.SetFloat("material.shineness", 32.0f);
        }

    }

}
