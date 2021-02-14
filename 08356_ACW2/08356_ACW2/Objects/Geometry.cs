using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Assimp;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;
//using Assimp;

namespace OpenGL_Game.Objects
{
    public class Geometry
    {
        private List<float> vertices = new List<float>();
        private int numberOfTriangles;

        // Graphics
        private int vao_Handle;
        private int[] vbo_verts = new int[2];

        /// <summary>
        /// this method essetinaly loops over the count of the vertices, gets the vertx array element
        /// for the vector 3d, gets the 3d vector and adds it to the list
        /// 
        /// </summary>
        /// <param name="vertList"></param>
        /// <param name="indices"></param>
        /// <param name="vertexCount"></param>
        public Geometry(List<Vector3D[]> vertList, int[] indices, int vertexCount)
        {
            numberOfTriangles = indices.Length;
            //takes the number of vertices to be loaded in per mesh sectioon
            for (int i = 0; i < vertexCount; i++)
            {
                //loops over a list of arrays to keep the data in order
                for (int j = 0; j < vertList.Count; j++)
                {
                    Vector3D[] vert = vertList[j];
                    Vector3D k = vert[i];
                    vertices.Add(k.X);
                    vertices.Add(k.Y);
                    //ignores the 3rd element for the tex coord as there is only two and would be a large waste of memory on larger models
                    if (j != 1)
                        vertices.Add(k.Z);

                }
            }
            try
            {
                GL.GenBuffers(2, vbo_verts);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_verts[0]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * sizeof(float)), vertices.ToArray<float>(), BufferUsageHint.StaticDraw);
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out int size);
                if (vertices.Count * sizeof(float) != size)
                {
                    throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
                }

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo_verts[1]);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)),
                indices, BufferUsageHint.StaticDraw);
                GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
                if (indices.Length * sizeof(int) != size)
                {
                    throw new ApplicationException("Index data not loaded onto graphics card correctly");
                }

                int bufferSize = 14 * sizeof(float);
                // Positions
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, bufferSize, 0);

                // Tex Coords
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, bufferSize, 3 * sizeof(float));

                // Normals
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, bufferSize, 5 * sizeof(float));

                // BiTan
                GL.EnableVertexAttribArray(3);
                GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, bufferSize, 8 * sizeof(float));

                // Tan
                GL.EnableVertexAttribArray(4);
                GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, bufferSize, 11 * sizeof(float));

                GL.BindVertexArray(0);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
        public void DeleteGeometry()
        {
            GL.DeleteBuffers(vbo_verts.Length, vbo_verts);
        }
        public void Render()
        {

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_verts[0]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo_verts[1]);

            // shader linking goes here
            GL.DrawElements(PrimitiveType.Triangles, numberOfTriangles, DrawElementsType.UnsignedInt, 0);


        }
    }
}
