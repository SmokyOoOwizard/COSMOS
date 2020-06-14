namespace COSMOS.Core
{
    public abstract class ABehaviour
    {
        public Actor Actor { get; private set; }
        public bool Active { get; private set; }

        public ABehaviour(Actor actor)
        {
            Actor = actor;
        }
        public void SetActive(bool active)
        {
            if (!Active && active)
            {
                OnEnable();
            }
            else if (Active && !active)
            {
                OnDisable();
            }
            Active = active;
        }

        public virtual void Awake()
        {

        }
        public virtual void Start()
        {

        }
        protected virtual void OnDisable()
        {

        }
        protected virtual void OnEnable()
        {

        }
        public virtual void OnDestroy()
        {

        }
    }
}
