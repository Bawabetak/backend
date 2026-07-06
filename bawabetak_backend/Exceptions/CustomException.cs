using bawabetak_backend.Enums;
using bawabetak_backend.Exceptions;
using Microsoft.AspNetCore.Http;

namespace bawabetak_backend.Helpers.Errors;

public class BadRequestCustomException : BaseException
{
    public BadRequestCustomException(ResponseKeys responseKey)
        : base(responseKey, StatusCodes.Status400BadRequest) { }
}

public class NotFoundCustomException : BaseException
{
    public NotFoundCustomException(ResponseKeys responseKey)
        : base(responseKey, StatusCodes.Status404NotFound) { }
}

public class UnauthorizedCustomException : BaseException
{
    public UnauthorizedCustomException(ResponseKeys responseKey)
        : base(responseKey, StatusCodes.Status401Unauthorized) { }
}