namespace SurveyBasket.API.Helpers
{
    public static class EmailBodyBuilder
    {
        public static string GenerateEmailBody(string template , Dictionary<string, string> templateModel)
        {
            var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
            var StreamReader = new StreamReader(templatePath);
            var body = StreamReader.ReadToEnd();

            foreach ( var item in templateModel )
            {
                body = body.Replace(item.Key, item.Value);
            }

            return body;
        }
    }
}
