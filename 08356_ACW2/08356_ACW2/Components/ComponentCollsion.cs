using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentCollsion : IComponent
    {
        private Vector3 linePointOne;
        private Vector3 linePointTwo;
        private Vector3 lineDistance;

        public ComponentCollsion()
        {
            linePointOne = new Vector3(0, 1, 0);
            linePointTwo = new Vector3(0, -1, 0);
            lineDistance = linePointOne - linePointTwo;
        }
        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_COLLSION;

        public Vector3 LinePointOne
        {
            get { return linePointOne; }
            set { linePointOne = value; }
        }
        public Vector3 LinePointTwo
        {
            get { return linePointTwo; }
            set { linePointTwo = value; }
        }
        public Vector3 LineDistance
        {
            get { return lineDistance; }
            set { lineDistance = value; }
        }
    }
}
