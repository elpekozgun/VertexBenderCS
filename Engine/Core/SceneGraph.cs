using Engine.GLApi;
using System;
using System.Collections.Generic;

namespace Engine.Core
{
    public class SceneGraph
    {
        public List<Transform> SceneItems;
        public Action<Transform> OnItemAdded;
        public Action<Transform> OnItemDeleted;
        public Action OnSceneCleared;

        public SceneTools SceneTools;

        public bool IsBlinnPhong;

        public bool ShowHeadLight { get; set; }
        public bool ShowDirectionalLight { get; set; }

        public float DirectionalLightX { get; set; }
        public float DirectionalLightY { get; set; }
        public float DirectionalLightZ { get; set; }

        public Transform SelectedItem;

        public SceneGraph()
        {
            SceneItems = new List<Transform>();
            IsBlinnPhong = true;
            ShowDirectionalLight = true;
            ShowHeadLight = false;
            SceneTools = new SceneTools();
            DirectionalLightX = (float)Math.PI;
        }

        public void Clean()
        {
            SceneItems.Clear();
            OnSceneCleared?.Invoke();
            SceneTools.Reset();
        }

        public void AddObject(Transform item)
        {
            SceneItems.Add(item);
            OnItemAdded?.Invoke(item);
            SceneTools.SetCartesianPosition(item.Position);
        }

        public void DeleteObject(Transform item)
        {
            foreach (var child in item.Children)
            {
                DeleteObject(child);
            }
            SceneItems.Remove(item);
            OnItemDeleted?.Invoke(item);
            SceneTools.Reset();
        }

        public void RenderAll(Camera cam, eRenderMode renderMode)
        {
            foreach (var item in SceneItems)
            {
                if (item.IsEnabled)
                {
                    if (item is IRenderable)
                    {
                        var renderable = item as IRenderable;

                        SetLights(renderable, cam);
                        renderable.Render(cam, renderMode);
                    }
                }
            }
            SceneTools.ShowDirectLightGizmo = ShowDirectionalLight;
            SceneTools.DirectLightDir = new OpenTK.Vector3(DirectionalLightX, DirectionalLightY, DirectionalLightZ);
            SceneTools.RenderAll(cam);
        }

        public void SetLights(IRenderable obj, Camera cam)
        {
            obj.Shader.Use();
            obj.Shader.SetVec3("cameraPosition", cam.Position);

            // TODO: Fix this problem. rotation is not same with the light object.
            if (ShowDirectionalLight)
            {
                var v = OpenTK.Matrix3.CreateRotationZ(DirectionalLightZ) *
                        OpenTK.Matrix3.CreateRotationY(DirectionalLightY) *
                        OpenTK.Matrix3.CreateRotationX(DirectionalLightX) * -OpenTK.Vector3.UnitY ;

                obj.Shader.SetVec3("directLight.direction",  v);
                obj.Shader.SetVec3("directLight.ambient", 0.25f, 0.25f, 0.25f);
                obj.Shader.SetVec3("directLight.diffuse", 0.5f, 0.5f, 0.4f);
                obj.Shader.SetVec3("directLight.specular", 0.1f, 0.1f, 0.1f);
            }
            else
            {
                obj.Shader.SetVec3("directLight.ambient", 0, 0, 0);
                obj.Shader.SetVec3("directLight.diffuse", 0, 0, 0);
                obj.Shader.SetVec3("directLight.specular", 0, 0, 0);
            }
            if (ShowHeadLight)
            {
                obj.Shader.SetVec3("spotlight.position", cam.Position);
                obj.Shader.SetVec3("spotlight.direction", cam.Front);
                obj.Shader.SetVec3("spotlight.ambient", 0.1f, 0.1f, 0.1f);
                obj.Shader.SetVec3("spotlight.diffuse", 0.4f, 0.4f, 0.4f);
                obj.Shader.SetVec3("spotlight.specular", 0.1f, 0.1f, 0.1f);
                obj.Shader.SetFloat("spotlight.cutOff", (float)Math.Cos(OpenTK.MathHelper.DegreesToRadians(12.5)));
                obj.Shader.SetFloat("spotlight.outerCutOff", (float)Math.Cos(OpenTK.MathHelper.DegreesToRadians(18.0)));
                obj.Shader.SetFloat("spotlight.Kconstant", 1.0f);
                obj.Shader.SetFloat("spotlight.Klinear", 0.027f);
                obj.Shader.SetFloat("spotlight.Kquad", 0.0028f);
            }
            else
            {
                obj.Shader.SetVec3("spotlight.ambient", 0, 0, 0);
                obj.Shader.SetVec3("spotlight.diffuse", 0, 0, 0);
                obj.Shader.SetVec3("spotlight.specular", 0, 0, 0);
            }

            //obj.Shader.SetVec3("pointLights[0].position", cam.Position);
            //obj.Shader.SetVec3("pointLights[0].ambient", 0.05f, 0.05f, 0.05f);
            //obj.Shader.SetVec3("pointLights[0].diffuse", 0.4f, 0.4f, 0.2f);
            //obj.Shader.SetVec3("pointLights[0].specular", 1.0f, 0.0f, 0.0f);
            //obj.Shader.SetFloat("pointLights[0].Kconstant", 1.0f);
            //obj.Shader.SetFloat("pointLights[0].Klinear", 0.09f);
            //obj.Shader.SetFloat("pointLights[0].Kquad", 0.032f);

            obj.Shader.SetBool("IsBlinnPhong", IsBlinnPhong);
            obj.Shader.SetFloat("material.shineness", 32.0f);
        }

    }

}
