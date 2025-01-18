using System.Net;
using Flunt.Notifications;

namespace EscalaApi.Services.Results;

public class Result : Notifiable<Notification>
{
    public bool Sucess { get { return !Notifications.Any(); } }
    public HttpStatusCode StatusCode { get; private set; }

    protected Result(IReadOnlyCollection<Notification> notifications, HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        AddNotifications(notifications);
    }

    protected Result(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    protected Result()
    {
    }

    public static Result Ok()
    {
        return new Result(HttpStatusCode.OK);
    }

    public static Result NoContent()
    {
        return new Result(HttpStatusCode.NoContent);
    }

    public static Result Created()
    {
        return new Result(HttpStatusCode.Created);
    }

    public static Result Accepted()
    {
        return new Result(HttpStatusCode.Accepted);
    }

    public static Result BadRequest(IReadOnlyCollection<Notification> notifications)
    {
        return new Result(notifications, HttpStatusCode.BadRequest);
    }

    public static Result NotFound(IReadOnlyCollection<Notification> notifications)
    {
        return new Result(notifications, HttpStatusCode.NotFound);
    }
}

public class Result<T> : Notifiable<Notification> where T : class
{
    public bool Sucess { get { return !Notifications.Any(); } }
    public T? Object { get; }
    public HttpStatusCode StatusCode { get; private set; }

    private Result(T obj, HttpStatusCode statusCode)
    {
        Object = obj;
        StatusCode = statusCode;
    }

    private Result(IReadOnlyCollection<Notification> notifications, HttpStatusCode statusCode)
    {
        Object = null;
        StatusCode = statusCode;
        AddNotifications(notifications);
    }

    public static Result<T> Ok(T obj)
    {
        return new Result<T>(obj, HttpStatusCode.OK);
    }

    public static Result<T> Created(T obj)
    {
        return new Result<T>(obj, HttpStatusCode.Created);
    }

    public static Result<T> NotFound(IReadOnlyCollection<Notification> notifications)
    {
        return new Result<T>(notifications, HttpStatusCode.NotFound);
    }

    public static Result<T> BadRequest(IReadOnlyCollection<Notification> notifications)
    {
        return new Result<T>(notifications, HttpStatusCode.BadRequest);
    }

    public static Result<T> InternalServerError(IReadOnlyCollection<Notification> notifications)
    {
        return new Result<T>(notifications, HttpStatusCode.InternalServerError);
    }
}