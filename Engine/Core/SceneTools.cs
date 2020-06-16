using Engine.GLApi;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Engine.Core
{
    public class SceneTools
    {
        public List<Transform> SceneItems;
        public GizmoRenderer VolumeEditor;
        public GridRenderer Grid;
        public GizmoRenderer CartesianX;
        public GizmoRenderer CartesianY;
        public GizmoRenderer CartesianZ;
        public GizmoRenderer CartesianCore;
        public GizmoRenderer BoundingBox;
        public GridRenderer DirectLight;

        public bool ShowGrid;

        public Vector3 DirectLightDir { get; set; }
        public bool ShowDirectLightGizmo { get; set; }

        public void SetCartesianPosition(Vector3 position)
        {
            CartesianCore.Position = position;
            CartesianX.Position = position;
            CartesianY.Position = position;
            CartesianZ.Position = position;

            CartesianCore.IsEnabled = true;
            CartesianX.IsEnabled = true;
            CartesianY.IsEnabled = true;
            CartesianZ.IsEnabled = true;
        }
        
        public void SetCartesianScale(float val)
        {
            CartesianCore.Scale = Vector3.One * val;
            CartesianX.Scale = Vector3.One * val;
            CartesianY.Scale = Vector3.One * val;
            CartesianZ.Scale = Vector3.One * val;

            DirectLight.Scale = Vector3.One * val * 10;
        }

        public void Reset()
        {
            CartesianCore.IsEnabled = false;
            CartesianX.IsEnabled = false;
            CartesianY.IsEnabled = false;
            CartesianZ.IsEnabled = false;
        }

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
            ShowGrid = true;

            var arrow = PrimitiveObjectFactory.Cylinder(0.25f, 0.25f, 4.0f, 48);

            CartesianX = new GizmoRenderer(arrow, Shader.Indicator)
            {
                Scale = new Vector3(0.1f, 0.1f, 0.1f),
                Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, -(float)Math.PI / 2),
                Color = new Vector4(1, 0.2f, 0, 1)
            };
            SceneItems.Add(CartesianX);
            CartesianX.IsEnabled = false;

            CartesianY = new GizmoRenderer(arrow, Shader.Indicator)
            {
                Scale = new Vector3(0.1f, 0.1f, 0.1f),
                Color = new Vector4(0, 1, 0.2f, 1)
            };
            SceneItems.Add(CartesianY);
            CartesianY.IsEnabled = false;

            CartesianZ = new GizmoRenderer(arrow, Shader.Indicator)
            {
                Scale = new Vector3(0.1f, 0.1f, 0.1f),
                Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, (float)Math.PI / 2),
                Color = new Vector4(0.2f, 0, 1, 1)
            };
            SceneItems.Add(CartesianZ);
            CartesianZ.IsEnabled = false;

            CartesianCore = new GizmoRenderer(PrimitiveObjectFactory.Sphere(0.5f, 4, eSphereGenerationType.Cube), Shader.Indicator)
            {
                Scale = new Vector3(0.1f, 0.1f, 0.1f),
                Color = new Vector4(0.4f, 0.4f, 0.4f, 1.0f)
            };
            SceneItems.Add(CartesianCore);
            CartesianCore.IsEnabled = false;

            Grid = new GridRenderer(PrimitiveObjectFactory.Grid(10, 0, 10, 1), Shader.Indicator)
            {
                EnableBlend = true,
                Color = new Vector4(0.4f, 0.4f, 0.4f, 0.5f)
            };
            SceneItems.Add(Grid);
            Grid.IsEnabled = true;

            DirectLight = new GridRenderer(PrimitiveObjectFactory.ConeForLighting(0.1f, 1, 16), Shader.Indicator)
            {
                Rotation = Quaternion.FromEulerAngles(DirectLightDir),
                Position = Vector3.One * 2,
                Color = new Vector4(0.8f, 0.8f, 0.2f, 1.0f),
                EnableCull = false
            };
            SceneItems.Add(DirectLight);
            DirectLight.IsEnabled = true;
        }

        public void Clean()
        {
            SceneItems.Clear();
        }

        public void RenderAll(Camera cam)
        {
            Grid.ShowGrid = ShowGrid;
            SetCartesianScale(0.01f * Vector3.Distance(CartesianCore.Position, cam.Position));
            DirectLight.Rotation = Quaternion.FromEulerAngles(DirectLightDir);
            DirectLight.IsEnabled = ShowDirectLightGizmo;

            foreach (var item in SceneItems)
            {
                if (item.IsEnabled)
                {
                    if (item is IRenderable)
                    {
                        var renderable = item as IRenderable;
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
