namespace Syki.Back.Services;

public interface INotificationsService
{
    Task<NotificationOut> Create(Guid faculdadeId, NotificationIn data);
    Task ViewByUserId(Guid faculdadeId, Guid userId);
    Task<List<NotificationOut>> GetAll(Guid faculdadeId);
    Task<List<UserNotificationOut>> GetByUserId(Guid faculdadeId, Guid userId);
}
