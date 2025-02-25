using Application.DraftTasks.CreateDraftTask;

namespace Application.DraftTasks.DeleteDraftTask;

public interface IDeleteDraftTaskService
{
    public  Task DeleteDraftTaskAsync(DeleteDraftTaskCommand command);
}