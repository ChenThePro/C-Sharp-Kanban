namespace Frontend.Model
{
    public abstract class NotifiableModelObject<TController> : NotifiableObject
    {
        public TController Controller { get; }

        protected NotifiableModelObject(TController controller) =>
            Controller = controller;
    }
}