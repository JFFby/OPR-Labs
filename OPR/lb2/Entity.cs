using OPR.lb1;
using OPR.lb2.Interfaces;

namespace OPR.lb2
{
    public class Entity<T> where T : IGenom, new()
    {
        public Entity() { }

        public Entity(Point<float> point)
        {
            Genom.Initialize(point);
        }

        protected Entity(T genom)
        {
            this.Genom = genom;
        }

        public T Genom = new T();
    }
}
