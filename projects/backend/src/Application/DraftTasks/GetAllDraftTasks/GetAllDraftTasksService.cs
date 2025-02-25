namespace Application.DraftTasks.GetAllDraftTasks;

public class GetAllDraftTasksService : IGetAllDraftTasksService
{
    private readonly IDraftTaskRepository _repository;

    public GetAllDraftTasksService(IDraftTaskRepository repository)
        => _repository = repository;

    public async Task<List<DraftTask>> GetAllDraftTasksAsync()
        => await _repository.GetAllAsync();
}