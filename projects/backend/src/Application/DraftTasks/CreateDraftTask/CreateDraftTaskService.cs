namespace Application.DraftTasks.CreateDraftTask;

public class CreateDraftTaskService : ICreateDraftTaskService
{
    private IDraftTaskRepository _repository;

    public CreateDraftTaskService(IDraftTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateDraftTask(CreateDraftTaskCommand command)
    {
        var draftTask = new DraftTask(command.Title, command.Priority, command.Description, command.Deadline,
            command.Duration);
        await _repository.AddAsync(draftTask);

        return draftTask.Id;
    }
}