using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.CustomAttributies
{
    //[AttributeUsage(AttributeTargets.All)] //كله 
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MinAgeAttributies : ValidationAttribute
    {
        private readonly int _minAge ;

        public MinAgeAttributies(int MinAge)
        {
            _minAge = MinAge;
        }
        //public override bool IsValid(object? value)
        //{
        //    if(value is not null)
        //    {
        //        var date = (DateTime)value;       //عشان متبقاش رقم ثابت
        //        if (DateTime.Today < date.AddYears(_minAge))//هاخد الانبوت اضيف عليه 18 لو طلع اكبر من دوقتي يبقي اتولد ف المستقبل مينفعش
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}


        //[ErrorMassage("Date Of Birth")] لو حابب اني مبعتش الماسج في ال 
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is not null)
            {
                var date = (DateTime)value;
                if(DateTime.Today < date.AddYears(_minAge))
                {
                    return new ValidationResult($"Invalid {validationContext.DisplayName} Age Should be {_minAge} years old");
                }
            }
            return ValidationResult.Success;
        }
    }
}
