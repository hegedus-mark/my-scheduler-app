namespace Application.DraftTasks.CreateDraftTask;

public interface ICreateDraftTaskService
{
    public Task<Guid>  CreateDraftTask(CreateDraftTaskCommand command);
}