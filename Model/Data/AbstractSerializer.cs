namespace Model.Data
{
    public abstract class AbstractSerializer<T>
    {
        public abstract void Serialize(string filePath, T data);
        public abstract T Deserialize(string filePath);
    }
}
