using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Engine.GLApi;

namespace Engine.Core
{
    public class SceneTools
    {
        public List<Transform> SceneItems;
        public GizmoRenderer VolumeEditor;
        public GizmoRenderer Grid;
        public GizmoRenderer CartesianX;
        public GizmoRenderer CartesianY;
        public GizmoRenderer CartesianZ;

        public SceneTools()
        {
            SceneItems = new List<Transform>();

            VolumeEditor = new GizmoRenderer(PrimitiveObjectFactory.Sphere(1, 4, eSphereGenerationType.Cube), Shader.EditorSphere, "VolumeEditor")
            {
                Scale = new Vector3(0.01f, 0.01f, 0.01f),
                EnableBlend = true,
                EnableCull = true,
                EnableDepth = false,
                Color = new Vector4(1.0f, 0.4f, 0.3f, 0.5f)
            };
            SceneItems.Add(VolumeEditor);
            VolumeEditor.IsEnabled = false;

            CartesianX = new GizmoRenderer(PrimitiveObjectFactory.Arrow3D(0.25f, 2.0f, 24), Shader.Indicator)
            {
                Scale = new Vector3(0.1f, 0.1f, 0.1f),
                Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, -(float)Math.PI / 2),
                EnableBlend = false,
                EnableCull = false,
                EnableDepth = false,
                Color = new Vector4(1, 0, 0, 1)
                //Color = new Vector4(1.0f, 0.4f, 0.3f, 0.5f)
            };
            SceneItems.Add(CartesianX);
            CartesianX.IsEnabled = true;

            CartesianY = new GizmoRenderer(PrimitiveObjectFactory.Arrow3D(0.25f, 2.0f, 24), Shader.Indicator)
            {
                Scale = new Vector3(0.1f, 0.1f, 0.1f),
                EnableBlend = false,
                EnableCull = false,
                EnableDepth = false,
                Color = new Vector4(0, 1, 0, 1)
                //Color = new Vector4(1.0f, 0.4f, 0.3f, 0.5f)
            };
            SceneItems.Add(CartesianY);
            CartesianY.IsEnabled = true;

            CartesianZ = new GizmoRenderer(PrimitiveObjectFactory.Arrow3D(0.25f, 2.0f, 24), Shader.Indicator)
            {
                Scale = new Vector3(0.1f, 0.1f, 0.1f),
                Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, (float)Math.PI / 2),
                EnableBlend = false,
                EnableCull = false,
                EnableDepth = false,
                Color = new Vector4(0, 0, 1, 1)
                //Color = new Vector4(1.0f, 0.4f, 0.3f, 0.5f)
            };
            SceneItems.Add(CartesianZ);
            CartesianZ.IsEnabled = true;



        }

        public void Clean()
        {
            SceneItems.Clear();
        }

        public void AddObject(Transform item)
        {
            SceneItems.Add(item);
        }

        public void DeleteObject(Transform item)
        {
            foreach (var child in item.Children)
            {
                DeleteObject(child);
            }
            SceneItems.Remove(item);
        }

        public void RenderAll(Camera cam)
        {
            foreach (var item in SceneItems)
            {
                if (item.IsEnabled)
                {
                    if (item is IRenderable)
                    {
                        var renderable = item as IRenderable;
                        SetDirectLight(renderable, cam);
                        renderable.Render(cam, eRenderMode.shaded);
                    }
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
            obj.Shader.SetBool("IsBlinnPhong", true);
            obj.Shader.SetFloat("material.shineness", 32.0f);
        }

    }
}
