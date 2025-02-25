namespace Application.DraftTasks.UpdateDraftTask;

public interface IUpdateDraftTaskService
{
    public Task UpdateDraftTaskAsync(UpdateDraftTaskCommand command);
}