namespace Application.DraftTasks.GetAllDraftTasks;

public interface IGetAllDraftTasksService
{
    Task<List<DraftTask>> GetAllDraftTasksAsync();
}