
public delegate void BackToPoolDelegate();

public interface ISpawnableObject
{
    event BackToPoolDelegate BackToPool;
    void InvokeBackToPool();
    int Type { get; set; }
}
