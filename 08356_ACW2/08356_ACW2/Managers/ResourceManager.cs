using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    static class ResourceManager
    {
        static Dictionary<string, Geometry> geometryDictionary = new Dictionary<string, Geometry>();
        static Dictionary<string, int> textureDictionary = new Dictionary<string, int>();
        static int[] mVertexArrayObjectIDs;
        private static int bufferCount = 0;

        public static void BindBufferArray(List<ModelObject> modelList, int modelCount)
        {
           
            foreach (var item in modelList)
            {
                bufferCount += item.GetNumberOfMeshes;
            }

            mVertexArrayObjectIDs = new int[bufferCount];
            GL.GenBuffers(bufferCount, mVertexArrayObjectIDs);
            int loopCount = 0;

            for (int i = 0; i < modelList.Count; i++)
            {
                Geometry[] geo = modelList[i].GetGeometry;
                for (int j = 0; j < geo.Length; j++)
                {
                   
                    geo[j].BindGeometry(loopCount, mVertexArrayObjectIDs);
                    loopCount++;
                }
            }
            
        }
        public static int LoadTexture(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int texture;
            textureDictionary.TryGetValue(filename, out texture);
            if (texture == 0)
            {
                texture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);

                // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
                // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
                // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                Bitmap bmp = new Bitmap(filename);
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }
 
            return texture;
        }
        public static void DeleteBuffers()
        {
            GL.DeleteVertexArrays(bufferCount, mVertexArrayObjectIDs);
        }
    }
}
