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
using System.Threading;

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
        private ModelManager modelManager;
        private ShaderManager shaderManager;
        private MaterialManager matManager;
        private List<TextureObject> textureObjectList = new List<TextureObject>();
        private List<MaterialObject> matObjectList = new List<MaterialObject>();

        public static MyGame gameInstance;

        public MyGame() 
            : base(1200, // Width
                900, // Height
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
            matManager = new MaterialManager("Textures/TextureList.txt");
            shaderManager = new ShaderManager("Shaders/ShaderList.txt");
            modelManager = new ModelManager("Geometry/ModelList.txt");
        }

        private void CreateEntities()
        {
            Entity newEntity;

            newEntity = new Entity("tower");
            //
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, -2), new Vector3(1,1,1), new Vector3(0,0,0)));
            newEntity.AddComponent(new ComponentModel(modelManager.FindModel("smallSphere")));
            newEntity.AddComponent(new ComponentMaterial(matManager.FindMaterial("scene")));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("tower");
            newEntity.AddComponent(new ComponentTransform(new Vector3(2, 0, -2), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentModel(modelManager.FindModel("cube")));
            newEntity.AddComponent(new ComponentMaterial(matManager.FindMaterial("scene")));
            entityManager.AddEntity(newEntity);

            //newEntity = new Entity("sphere");
            //newEntity.AddComponent(new ComponentPosition(2.0f, 0.0f, -3.0f));
            //// newEntity.AddComponent(new ComponentGeometry("Geometry/Cube.fbx"));
            ////newEntity.AddComponent(new ComponentMaterial(FindMaterial("scene")));
            //// entityManager.AddEntity(newEntity);
            //float g = (Vector3.Dot(new Vector3(1,2,3), new Vector3(1,1,1) * new Vector3(2,2,2)));
            //Vector3 t = new Vector3(1, 2, 3);
            //float a = t.Length;

        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender(shaderManager.FindShader("pbrShader"));
            systemManager.AddSystem(newSystem);
        }
        /// <summary>
        /// Allows the game to setup the environment and matrices.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.CullFace);
            view = Matrix4.LookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), 800f / 480f, 0.01f, 100f);
            
            modelManager.BindGeometetry();
            CreateSystems();
           
            CreateEntities();
         

            // TODO: Add your initialization logic here
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
           
           
            shaderManager.DeleteShaders();
            modelManager.DeleteBuffers();
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
