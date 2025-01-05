namespace Application.Shared.Exceptions;

public class MissingCommandHandlerException(string handlerTypeName)
    : ApplicationException(
        $"Command handler of type '{handlerTypeName}' is not registered in the service container"
    ) { }
