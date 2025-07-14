namespace SurveyBasket.API.Repositories;

public interface INotificationService
{
    Task SendNewPollsNotification(int? pollId = null);
}