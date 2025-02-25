namespace Application.DraftTasks;

public class DraftTask
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public TimeSpan? Duration { get; private set; }
    public Priority Priority { get; private set; }
    public DateTime? Deadline { get; private set; }

    public DraftTask(string title, Priority priority, string? description, DateTime? deadline, TimeSpan? duration)
    {
        Id = Guid.NewGuid();
        SetTitle(title);
        SetPriority(priority);
        SetDescription(description);
        SetDeadline(deadline);
        SetDuration(duration);
    }

    private DraftTask()
    {
        //For EF    
    }


    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));
        }

        Title = title;
    }

    public void SetDescription(string? description)
    {
        // Description can be null or empty, but if it's not null, it shouldn't be just whitespace.
        if (description != null && string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be whitespace.", nameof(description));
        }

        Description = description;
    }

    public void SetDuration(TimeSpan? duration)
    {
        // Duration can be null, but if it's not null, it should be a positive value.
        if (duration.HasValue && duration.Value <= TimeSpan.Zero)
        {
            throw new ArgumentException("Duration must be a positive value.", nameof(duration));
        }

        Duration = duration;
    }

    public void SetPriority(Priority priority)
    {
        Priority = priority;
    }

    public void SetDeadline(DateTime? deadline)
    {

        Deadline = deadline;
    }
}