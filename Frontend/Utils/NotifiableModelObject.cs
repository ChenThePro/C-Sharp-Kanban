namespace Frontend.Utils
{
    public abstract class NotifiableModelObject<TController> : NotifiableObject
    {
        public TController Controller { get; init; }

        protected NotifiableModelObject(TController controller) =>
            Controller = controller;
    }
}