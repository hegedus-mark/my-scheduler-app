using Application.DraftTasks.CreateDraftTask;

namespace Application.DraftTasks.DeleteDraftTask;

public class DeleteDraftTaskService : IDeleteDraftTaskService
{
    private IDraftTaskRepository _repository;

    public DeleteDraftTaskService(IDraftTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task DeleteDraftTaskAsync(DeleteDraftTaskCommand command)
    {
        var draftTask = await _repository.GetByIdAsync(command.DraftTaskId);

        if (draftTask is null)
        {
            throw new InvalidOperationException("Id not found");
        }
        
        await _repository.DeleteAsync(draftTask);
    }
}