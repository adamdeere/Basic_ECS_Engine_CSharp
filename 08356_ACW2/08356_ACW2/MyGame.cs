using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK.Graphics;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace OpenGL_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyGame : GameWindow
    {
        public Matrix4 view, projection;
        private EntityManager entityManager;
        private SystemManager systemManager;
        private List<ModelObject> modelObjectList = new List<ModelObject>();
        private List<TextureObject> textureObjectList = new List<TextureObject>();
        private List<Material> matObjectList = new List<Material>();

        public static MyGame gameInstance;

        public MyGame() 
            : base(800, // Width
                600, // Height
                GraphicsMode.Default,
                "Component based tower",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
         
        }

        private void CreateEntities()
        {
            Entity newEntity;

            newEntity = new Entity("tower");
            newEntity.AddComponent(new ComponentPosition(-2.0f, -1.0f, -4.0f));
            newEntity.AddComponent(new ComponentGeometry(FindModels("tower")));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("cylinder");
            newEntity.AddComponent(new ComponentPosition(-2.0f, -1.0f, -4.0f));
           // newEntity.AddComponent(new ComponentGeometry("Geometry/CubeSphere.fbx"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("sphere");
            newEntity.AddComponent(new ComponentPosition(2.0f, 0.0f, -3.0f));
           // newEntity.AddComponent(new ComponentGeometry("Geometry/Cube.fbx"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
           // entityManager.AddEntity(newEntity);
            float g = (Vector3.Dot(new Vector3(1,2,3), new Vector3(1,1,1) * new Vector3(2,2,2)));
            Vector3 t = new Vector3(1, 2, 3);
            float a = t.Length;
            
        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
        }

        private void LoadModels()
        {
            //reads all of the models in from a file and pops them into a list with a tag so that they can be found and used by whichever object needs it
            using (StreamReader modelSR = new StreamReader("Geometry/ModelList.txt"))
            {
               
                while (modelSR.Peek() > -1)
                {

                    string line = modelSR.ReadLine();
                    string[] result = line.Split(new string[] { "\n", "\r\n", "," }, StringSplitOptions.RemoveEmptyEntries);
                    modelObjectList.Add(new ModelObject(result[0], "Geometry/" + result[1]));
                }
            }
        }
        private ModelObject FindModels(string modelTag)
        {
            for (int i = 0; i < modelObjectList.Count; i++)
            {
                if (modelObjectList[i].GetModelTag == modelTag)
                {
                    return modelObjectList[i];
                }
            }
            //if we get here, the model has not been found in the list and should be handled accordingly
            throw new ArgumentException("could not find model");
        }
        private void LoadTextures()
        {
            //reads in all of the textures from a file list
            using (StreamReader textureSR = new StreamReader("Textures/TextureList.txt"))
            {
                while (textureSR.Peek() > -1)
                {

                    string line = textureSR.ReadLine();
                    string[] result = line.Split(new string[] { "\n", "\r\n", "," }, StringSplitOptions.RemoveEmptyEntries);
                    TextureObject texObject = new TextureObject(result[0], "Textures/" + result[1]);

                    textureObjectList.Add(texObject);
                    SortTextures(texObject);
                  
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
                matObjectList.Add(new Material(texObj.GetTextureTag));
            }
            //if we get here, we can assume that we are at the start of the list and we need to create an entry into the matrial manager
            else
            {
                matObjectList.Add(new Material(texObj.GetTextureTag));
                matObjectList[0].AddTexture(texObj);
            }
            
        }
        private void FindTextures()
        {

        }

        /// <summary>
        /// Allows the game to setup the environment and matrices.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            view = Matrix4.LookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), 800f / 480f, 0.01f, 100f);
            LoadModels();
            LoadTextures();
            CreateEntities();
            CreateSystems();

            // TODO: Add your initialization logic here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Key.Escape))
                Exit();

            // TODO: Add your update logic here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            systemManager.ActionSystems(entityManager);

            GL.Flush();
            SwapBuffers();
        }
    }
}
