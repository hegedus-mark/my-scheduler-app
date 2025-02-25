namespace Application.DraftTasks.UpdateDraftTask;

public class UpdateDraftTaskService : IUpdateDraftTaskService
{

    private IDraftTaskRepository _repository; 
    
    
    public UpdateDraftTaskService(IDraftTaskRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task UpdateDraftTaskAsync(UpdateDraftTaskCommand command)
    {
        var draftTask = await _repository.GetByIdAsync(command.DraftTaskId);

        if (draftTask is null)
        {
            throw new InvalidOperationException("Id not found");
        }

        if (command.Deadline.HasValue)
        {
            draftTask.SetDeadline(command.Deadline);
        }

        if (command.Priority.HasValue)
        {
            draftTask.SetPriority(command.Priority.Value);
        }

        if (command.Title != null)
        {
            draftTask.SetTitle(command.Title);
        }

        if (command.Description != null)
        {
            draftTask.SetDescription(command.Description);
        }

        if (command.Duration.HasValue)
        {
            draftTask.SetDuration(command.Duration.Value);
        }

        await _repository.UpdateAsync(draftTask);

    }

}