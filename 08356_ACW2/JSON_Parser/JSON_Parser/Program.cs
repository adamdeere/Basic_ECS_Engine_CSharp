using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JSON_Parser
{
    class Program
    {
      
        static void Main(string[] args)
        {
            List<SceneData> m_SceneData = new List<SceneData>();
          
            List<ModelData> m_ModelSata = new List<ModelData>();
            using StreamReader textureSR = new StreamReader("Files/TowerTouchUp.txt");
            while (textureSR.Peek() > -1)
            {

                string line = textureSR.ReadLine();
                string[] result = line.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] dataLine = result[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                m_SceneData.Add(new SceneData(dataLine[0], dataLine[2], dataLine[4], dataLine[6]));
             
            }
            List<SceneData> copy_List = m_SceneData.Distinct().ToList();
            for (int i = 0; i < copy_List.Count; i++)
            {
               
            }
            
            string JsonResult = JsonConvert.SerializeObject(m_SceneData, Formatting.Indented);
            using var tw = new StreamWriter(@"C:\Users\adamd\OneDrive\Documents\GitHub\SceneData.json", true);
            tw.WriteLine(JsonResult.ToString());
            tw.Close();
        }
    }
}
