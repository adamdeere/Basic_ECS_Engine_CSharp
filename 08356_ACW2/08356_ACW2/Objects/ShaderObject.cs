using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Objects
{
    class ShaderObject
    {
        private string m_Tag;

        private int pgmID;
        private int vsID;
        private int fsID;
        private int attribute_vtex;
        private int attribute_vpos;

        private int attribute_vNorm;
        private int attribute_vBiTan;
        private int attribute_vTan;

        protected int uniform_stex;
        protected int uniform_mview;
        public ShaderObject(string tag, string vs, string fs)
        {
            m_Tag = tag;
            pgmID = GL.CreateProgram();
            LoadShader($"Shaders/{vs}", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader($"Shaders/{fs}", ShaderType.FragmentShader, pgmID, out fsID);


            GL.LinkProgram(pgmID);

            attribute_vpos = GL.GetAttribLocation(pgmID, "a_Position");
            attribute_vtex = GL.GetAttribLocation(pgmID, "a_TexCoord");

            attribute_vNorm = GL.GetAttribLocation(pgmID, "a_Normal");
            attribute_vBiTan = GL.GetAttribLocation(pgmID, "a_biTan");
            attribute_vTan = GL.GetAttribLocation(pgmID, "a_Tan");


            uniform_mview = GL.GetUniformLocation(pgmID, "WorldViewProj");

            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");

            if (attribute_vpos == -1 || attribute_vtex == -1 || uniform_stex == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }
            if (attribute_vNorm == -1 || attribute_vBiTan == -1 || attribute_vTan == -1)
            {
                Console.WriteLine("Error binding attributes");
            }
        }
        void LoadShader(string filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.GetShader(address, ShaderParameter.CompileStatus, out int result);
            if (result == 0)
            {
                throw new Exception("Failed to compile shader!" + GL.GetShaderInfoLog(address));
            }
            GL.AttachShader(program, address);

        }
        public void DeleteShader()
        {
            GL.DetachShader(pgmID, vsID);
            GL.DetachShader(pgmID, fsID);
            GL.DeleteShader(vsID);
            GL.DeleteShader(fsID);
            GL.DeleteProgram(pgmID);
        }
        public string GetTag => m_Tag;

        public int GetProgramId => pgmID;
        public int GetVSid =>vsID;
        public int GetFSid => fsID;
        public int GetVtexAtt => attribute_vtex;
        public int GetPos => attribute_vpos;

        public int GetNormAtt => attribute_vNorm;
        public int GetBiTanAtt => attribute_vBiTan;
        public int GetVTanAtt => attribute_vTan;

        public int GetUniformStex => uniform_stex;
        public int GetuniformMView => uniform_mview;
    }
}
