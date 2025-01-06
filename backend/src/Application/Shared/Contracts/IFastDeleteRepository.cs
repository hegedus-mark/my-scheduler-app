namespace Application.Shared.Contracts;

public interface IFastDeleteRepository
{
    void FastDeleteById(Guid id);
}
