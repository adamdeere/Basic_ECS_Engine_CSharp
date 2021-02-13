using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK.Graphics;

namespace OpenGL_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyGame : GameWindow
    {
        public Matrix4 view, projection;
        EntityManager entityManager;
        SystemManager systemManager;

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

            newEntity = new Entity("Triangle1");
            newEntity.AddComponent(new ComponentPosition(-2.0f, -1.0f, -4.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeSphere.fbx"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Square1");
            newEntity.AddComponent(new ComponentPosition(2.0f, 0.0f, -3.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Cube.fbx"));
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

        /// <summary>
        /// Allows the game to setup the environment and matrices.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            view = Matrix4.LookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), 800f / 480f, 0.01f, 100f);

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
