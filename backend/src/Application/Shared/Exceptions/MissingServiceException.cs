namespace Application.Shared.Exceptions;

public class MissingHandlerException(string handlerTypeName)
    : ApplicationException(
        $"Handler of type '{handlerTypeName}' is not registered in the service container"
    ) { }
