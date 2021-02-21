using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Objects
{
    public class ShaderObject
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

        private int uniform_albeidoTexture;
        private int uniform_normalTexture;
        private int uniform_heightTexture;
        private int uniform_metalicTexture;
        private int uniform_roughnessTexture;
        private int uniform_MVP_Matrix;
        private int uniform_ViewMatrix;
        private int uniform_ModelMatrix;
        private int uniform_ProjectionMatrix;

        private int attribute_Eyepostion;
        private int attribute_LightPosition;



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


            attribute_Eyepostion = GL.GetUniformLocation(pgmID, "uEyePosition");
            attribute_LightPosition = GL.GetUniformLocation(pgmID, "uLightPosition");

            uniform_MVP_Matrix = GL.GetUniformLocation(pgmID, "WorldViewProj");

            uniform_albeidoTexture = GL.GetUniformLocation(pgmID, "s_texture");
            uniform_normalTexture = GL.GetUniformLocation(pgmID, "s_NormalTexture");
            uniform_heightTexture = GL.GetUniformLocation(pgmID, "s_HeightTexture");
            uniform_metalicTexture = GL.GetUniformLocation(pgmID, "s_MetalicTexture");
            uniform_roughnessTexture = GL.GetUniformLocation(pgmID, "s_RoughnessTexture");

            uniform_ViewMatrix = GL.GetUniformLocation(pgmID, "uView");
            uniform_ModelMatrix = GL.GetUniformLocation(pgmID, "uModel");
            uniform_ProjectionMatrix = GL.GetUniformLocation(pgmID, "uProjecttion"); ;


            if (attribute_Eyepostion == -1 || attribute_LightPosition == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            if (attribute_vpos == -1 || attribute_vtex == -1 || uniform_albeidoTexture == -1 || uniform_MVP_Matrix == -1)
            {
                Console.WriteLine("Error binding attributes");
            }
            if (attribute_vNorm == -1 || attribute_vBiTan == -1 || attribute_vTan == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            if (uniform_ViewMatrix == -1 || uniform_ModelMatrix == -1 || uniform_ProjectionMatrix == -1)
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

        public int GetUniformColourTex => uniform_albeidoTexture;
        public int GetUniformNormalTex => uniform_normalTexture;
        public int GetUniformHeightTex => uniform_heightTexture;
        public int GetUniformMetalicTex => uniform_metalicTexture;
        public int GetUniformRoughnessTex => uniform_roughnessTexture;


        public int GetuniforMVP_Matrix => uniform_MVP_Matrix;

        public int Get_uniform_ViewMatrix => uniform_ViewMatrix;
        public int Get_uniform_ModelMatrix => uniform_ModelMatrix;
        
        public int Get_uniform_ProjectionMatrix => uniform_ProjectionMatrix;

        public int Get_EyePosition => attribute_Eyepostion;

        public int Get_Lightposiytion => attribute_LightPosition;

    }
}
