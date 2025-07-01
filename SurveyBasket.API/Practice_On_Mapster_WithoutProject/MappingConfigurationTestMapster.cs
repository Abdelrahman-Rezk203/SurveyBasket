using Mapster;

namespace SurveyBasket.API.Practice_On_Mapster_WithoutProject
{
    public class MappingConfigurationTestMapster : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //الريكويست فيه كل الداتا انما الريسبونس عنده الاسم الكامل فقط فانا هقوله حط الدااتا من هناك
            config.NewConfig<StudentDtoRequest, StudentResponse>()
                .Map(X => X.FullName, Y => $"{Y.FirstName} {Y.MiddleName} {Y.LastName}")
                //كده  بقوله الاسم الكامل اللي ف الريكويست متسبهوش فاضي حد فيه كذه كذه عشان مش نفس الفاريابل

                //Value عشان عامل nullable
                .Map(A => A.Age, D => $"{(DateTime.Now.Year - D.DateOFBirth!.Value.Year)}",
                        Condition => Condition.DateOFBirth.HasValue)//ميرميش اكسبشن null عشان لو مبعتش قيمه يحط 
               .Ignore(Ign=>Ign.DepartmentName) ;

            //.Map(Z => Z.DepartmentName, Dep => Dep.Department.Name); //By Convention//هو كده كده هيحول تلقاي عشان الاسم مرتبط 

            //StudentDtoRequest         public DepartmentDtoRequest Department { get; set; } = default!;
            //DepartmentDtoRequest            public string Name { get; set; } = string.Empty;
            //relation one to one 
            //StudentResponse      public string DepartmentName { get; set; } = default!;



            //config.NewConfig<StudentDtoRequest, StudentResponse>().TwoWays(); العكس
            //OR
            //config.NewConfig<StudentDtoRequest, StudentResponse>();
            //config.NewConfig<StudentResponse,StudentDtoRequest>();








        }
    }
}
