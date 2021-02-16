using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Objects;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace OpenGL_Game.Managers
{
    class MaterialManager
    {
        private List<MaterialObject> matObjectList;
        private List<TextureObject> textureObjectList;

        public MaterialManager(string fileName)
        {
            matObjectList = new List<MaterialObject>();
            textureObjectList = new List<TextureObject>();
            using (StreamReader textureSR = new StreamReader(fileName))
            {
                while (textureSR.Peek() > -1)
                {

                    string line = textureSR.ReadLine();
                    string[] result = line.Split(new string[] { "\n", "\r\n", "," }, StringSplitOptions.RemoveEmptyEntries);
                    TextureObject texObject = new TextureObject(result[0], "Textures/" + result[1]);

                    textureObjectList.Add(texObject);
                    SortTextures(texObject);
                    //new Thread(() => { SortTextures(texObject); }).Start();
                }
            }
        }
        private void SortTextures(TextureObject texObj)
        {

            //checks to see if there is something in the list already for a comarison
            if (matObjectList.Count > 0)
            {
                for (int i = 0; i < matObjectList.Count; i++)
                {
                    if (matObjectList[i].GetMatTag == texObj.GetTextureTag)
                    {
                        matObjectList[i].AddTexture(texObj);
                        return;
                    }
                }
                matObjectList.Add(new MaterialObject(texObj.GetTextureTag));
                matObjectList[matObjectList.Count - 1].AddTexture(texObj);
              
            }
            //if we get here, we can assume that we are at the start of the list and we need to create an entry into the matrial manager
            else
            {
                matObjectList.Add(new MaterialObject(texObj.GetTextureTag));
                matObjectList[0].AddTexture(texObj);
            }

        }

        public MaterialObject FindMaterial(string tag)
        {
            foreach (var item in matObjectList)
            {
                if (item.GetMatTag == tag)
                {
                    return item;
                }
            }
            throw new Exception("Failed to compile shader!");
        }

        public void DeleteTextures()
        {
            for (int i = 0; i < textureObjectList.Count; i++)
            {
                GL.DeleteTexture(textureObjectList[i].GetTextureNumber);
            }
        }

    }
}
