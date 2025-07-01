namespace SurveyBasket.API.DtoRequestAndResponse.Question
{
    public record QuestionWithOutAnswerResponse
    (
        int Id,
        string Content,
        bool IsActive
    );
}
