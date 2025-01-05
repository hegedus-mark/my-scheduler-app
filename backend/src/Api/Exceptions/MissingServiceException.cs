namespace Api.Exceptions;

public class MissingServiceException(string missingServiceName)
    : ApiException($"{missingServiceName} is missing service from registration") { }
