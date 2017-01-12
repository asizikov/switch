namespace Switch
{
    public abstract class Feature : IOption
    {
        public bool IsEnabled { get; set; }
    }
}