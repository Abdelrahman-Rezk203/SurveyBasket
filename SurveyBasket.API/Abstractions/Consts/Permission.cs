namespace SurveyBasket.API.Abstractions.Consts
{
    public static class Permissions
    {
        public static string Type { get; } = "Permissions";

        //Authorize   هعمل اللي عليهم ال 
        public const string GetPoll   = "polls:read";
        public const string AddPoll   = "polls:Add";
        public const string UpdatePoll   = "polls:Update";
        public const string DeletePoll   = "polls:Delete";


        public const string GetQuestion   = "questions:read";
        public const string AddQuestion   = "questions:Add";
        public const string UpdateQuestion   = "questions:Update";


        public const string GetUsers   = "users:read";
        public const string AddUsers   = "users:add";
        public const string UpdateUsers  = "users:update";


        public const string getRoles = "roles:read";
        public const string addRoles = "roles:add";
        public const string updateRoles = "roles:update";

        public const string Result = "result:read";

        public static IList<string?> GetAllPermissions() =>
            typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();






    }
}
