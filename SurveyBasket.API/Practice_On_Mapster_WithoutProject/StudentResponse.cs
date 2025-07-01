namespace SurveyBasket.API.Practice_On_Mapster_WithoutProject
{
    public class StudentResponse //دا الرد اللي هيظهر لليوزر
    {

        public int Id { get; set; }
        //public string FirstName { get; set; } = string.Empty;
        //public string MiddleName { get; set; } = string.Empty;
        //public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty; //هيكون فاضي مش هعرف اعمل مابنج لازم اقوله هات القيم بتاعته منين
        //طاملا الداتا اختلفت لازم افهمه 

        public int? Age { get; set; } //هيحط فيه صفر لازم الاول اقوله خد العمر 

        public string DepartmentName { get; set; } = string.Empty;


    }
}
