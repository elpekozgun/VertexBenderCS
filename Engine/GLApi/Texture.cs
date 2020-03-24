using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace Engine.GLApi
{
    public enum eTextureType
    {
        Diffuse,
        Metallic,
        Specular,
        Normal,
        Height,
        Occlusion
    }

    public struct Texture
    {
        public int Id;
        public eTextureType Type;
        public string Path;

        public Texture(int id, eTextureType type, string path)
        {
            Id = id;
            Type = type;
            Path = path;
        }

        public static Texture LoadTexture(string path, eTextureType type)
        {
            Bitmap bmp = new Bitmap(path);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            bmp.Dispose();

            return new Texture(id, type, path);
        }

    }
}
