namespace RazorShop.Shared;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Seccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;


}