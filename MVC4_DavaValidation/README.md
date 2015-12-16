MVC4 �������
===

# �ϥ�DataAnnotation���Ѫ���������
MVC��DataAnnotation���ѤF���ت�����, �u�n�ޤJ��ϥ�DataType�w�q�n, �N�|�۰ʰ��e��ݪ�����.

### �ϥ�DataType=Email���d��
```
//Model
[DataType(DataType.EmailAddress)]
[StringLength(128)]
[Required]
public string Email { get; set; }
//HTML
<div class="editor-label">
    @Html.LabelFor(model => model.Email)
</div>
<div class="editor-field">
    @Html.EditorFor(model => model.Email)
    @Html.ValidationMessageFor(model => model.Email)
</div>
```

### �ϥ�ModelState.AddError
MVC�]���ѤF�bPOST��ƫ�, �b��ݥi�H�[�J�ۤv���ӷ~�޿�P�_��, �æۭq���~�T�����. �ӥB�@������FAddError, `ModelState.IsValid`�N�@�w�|�Ofalse.

```
//Model
public string AddErrorEmail { get; set; }
//Controller
public ActionResult Index(User model)
{
    if (model.AddErrorEmail.Equals("t@gmail.com"))
    {
        ModelState.AddModelError("ModelStateError", "Email���i�H�Ot@gmail.com");
    }
}
//Html
<div class="editor-label">
    @Html.LabelFor(model => model.AddErrorEmail)
</div>
<div class="editor-field">
    @Html.EditorFor(model => model.AddErrorEmail)
    @Html.ValidationMessage("ModelStateError")
</div>
```

---
# �إ߫Ȼs�ƫe������
�إ����ҿ�J���ͤ餣�i�j�󤵤���.

### �إߦۭq�ݩ�, ���~��IClientValidatable
```
public sealed class ValidBirthDate : ValidationAttribute, IClientValidatable
{
}
```

### ��@IClientValidatable
�~��IClientValidatable��, �ݹ�@`GetClientValidationRules`. ���e�p�U:
1. `ValidationType`�O�b����줤�|���ͪ�data, �Ψӵ��X`jquery.validate.unobtrusive`�ϥ�.
2. ErrorMessage�O�ΨӦbJS�����ҥ��ѫ�n���ͪ����~�T��.
```
public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
{
	var rule = new ModelClientValidationRule
	{
		ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
		ValidationType = "validbirthdate",
	};
	yield return rule;
}
```

### �e�ݷf�t��Javascript
�n���J�e�����Ҫ��禡�w(�w�g�bbundle���ŧi), �M��H��~�ҩw�q��ValidationType�[�J���Ҫ�adapter. �o�˴N�����e�����Ҫ��u�@.
```
@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")
    <script type="text/javascript">
        //1. �i�H�ϥ�$('#' + $("[name=BirthDate]").data('val-validbirthdate-para1')).val()�Ө��brule.ValidationParameters.Add("para1", OtherProperty);�w�q����
        //2. �i�H�ϥ�$("[name=BirthDate]").data('val-validbirthdate-other')�Ө��brule.ValidationParameters["other"] = "123";�w�q����
        jQuery.validator.addMethod('validbirthdate', function (value, element, params) {
            console.log($('#' + $("[name=BirthDate]").data('val-validbirthdate-para1')).val());
            console.log($("[name=BirthDate]").data('val-validbirthdate-other'));
            var currentDate = new Date();
            if (Date.parse(value) > currentDate) {
                return false;
            }
            return true;
        }, '');
        jQuery.validator.unobtrusive.adapters.add('validbirthdate', function(options) {
            options.rules['validbirthdate'] = {};
            options.messages['validbirthdate'] = options.message;
        });
    </script>
}
```

### �[�j�e�����Ҫ��\�� - �ϥΨ�L���ӧ@�P�_
�i�H�z�L�bModel�w�q�ɶǤJ��L���W�٪��r��, �M��b�e�ݷf�t����. ���p�b�e���ˬd��, �n���oAge��쪺�ȧ@����. �[�J�H�U�{���X.
���|�bhtml������data-val-validbirthdate-other. �ҥH�i�H�Q�γo�ؤ覡, �b�e�ݥHjQuery�h��Age��쪺��.
```
//Model
[Range(20, 40, ErrorMessage = "Customer Age should be between 20 to 40.")]
public int Age { get; set; }
[Display(Name = "Date of Birth")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
[ValidBirthDate("Age", ErrorMessage = "Birth Date can not be greater than current date")]
public DateTime BirthDate { get; set; }
//�ۭq�ݩ�: Age�r��|�ǤJ�æs�JOtherProperty
public sealed class ValidBirthDate : ValidationAttribute, IClientValidatable
{
    public string OtherProperty { get; set; }
    public ValidBirthDate(string otherProperty)
    {
        OtherProperty = otherProperty;
    }
}
//Attribute
public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
{
    var rule = new ModelClientValidationRule
    {
        ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
        ValidationType = "validbirthdate",
    };
    //�|�bhtml������data-val-validbirthdate-other. �ҥH�i�H�Q�γo�ؤ覡, �a�J�Ѽ�
    rule.ValidationParameters.Add("para1", OtherProperty);
    yield return rule;
}
//JS: ��para1(Age���W�٦r��)����(�N�OAge)
$('#' + $("[name=BirthDate]").data('val-validbirthdate-para1')).val()
```

���ͪ�html
![Alt text](GenerateHtml.png)

---
# �@�����h������
�i�H����ݤΫe����. (example: /MultiValidate)

### �������
�Ѧ�`Handling Multiple Validation Rules in ASP.NET MVC Using DataAnnotations and Unobtrusive jQuery Validator - Part 1`. ��P�@����찵3������.

1. �n���إ�3�ӿW�ߪ��Ȼs������: OnlyOneAttribute.cs, CannotContainNumbersAttribute.cs, CannotBeNotApplicableAttribute.cs
2. �A�إߤ@�ӦX�֭�~�إߪ���3�Ӫ��������O: ValidNameAttribute.cs
3. �إ�ValidateRegisterModel.cs��������O, FirstName���M��ValidNameAttribut
4. ��J`24&`�|�X�{2�ӦX�֪����~�T��

### �e������
�@�k�P������ҹp�P

1. �b3�ӿW�ߪ��Ȼs������(��~�إߪ���ݪ���3��)�[�W�e�����ҵ{��.
2. �bValidNameAttribut��@IClientValidatable.
3. �إ�rule��, �[�J3�ӰѼ�. �C�ӰѼƪ��ȳ��n��`,`�j�}, �b�e�ݷ|��split�h���o�U�۪��r��
  - clientvalidationrules: 3�ӿW�ߪ��Ȼs�ƫe�����ҵ{����ValidationType�W��
  - regexpatterns: �����Үɪ�regular expression, �n�ǵ��e�ݰ�����
  - errormessages: 3�ӿW�ߪ��Ȼs�ƫe�����ҵ{�������~�T��.
4. �b�e�ݥ[�WJS����code, �`�@�n��3�ӿW�ߪ��Ȼs�ƫe�����Ҥ�1�Ӻ�X�����ҵ{��.


```
rule.ValidationParameters.Add("clientvalidationrules", clientValidationRules.Aggregate((i, j) => i + "," + j));
rule.ValidationParameters.Add("regexpatterns", regexPatterns.Aggregate((i, j) => i + "," + j));
rule.ValidationParameters.Add("errormessages", errorMessages.Aggregate((i, j) => i + "," + j));
```

---
# ��L�p�ޥ�

### �e�ݦۤv�w�q���~�T��
���n�[options.messages, �M��b�P�_���ۤv�^�ǧY�i. �i�Φb�P�_���P����U�h��ܤ��P���T��.

```
jQuery.validator.addMethod('validbirthdate', function (value, element, params) {
    var currentDate = new Date();
    if (Date.parse(value) > currentDate) {
        jQuery.validator.messages.validbirthdate = "xxx";	//<-- �ۦ�^��
        return false;
    }
    return true;
}, '');
jQuery.validator.unobtrusive.adapters.add('validbirthdate', function(options) {
    options.rules['validbirthdate'] = {};
    //options.messages['validbirthdate'] = options.message;	<-- ���n�[options.messages
});
```

---
# ���Ϊ��ѦҸ��
* [�ڰѦҳo�g�إߪ�l�d��- ASP.NET MVC Model Validation using Data Annotations](https://joeylicc.wordpress.com/2013/06/20/asp-net-mvc-model-validation-using-data-annotations/)
* [�x�誺 - Using Data Annotations for Model Validation](http://www.asp.net/mvc/overview/older-versions/mvc-music-store/mvc-music-store-part-6)
* [�e�����Ҧ��\���d��-�ϥ�customer��-http://www.ezzylearning.com/tutorial/creating-custom-validation-attribute-in-asp-net-mvc](http://www.ezzylearning.com/tutorial/creating-custom-validation-attribute-in-asp-net-mvc)
* [Custom Data Annotation Validator Part I : Server Code](http://odetocode.com/blogs/scott/archive/2011/02/22/custom-data-annotation-validator-part-i-server-code.aspx)
* [Custom Data Annotation Validator Part II: Client Code](http://odetocode.com/blogs/scott/archive/2011/02/23/custom-data-annotation-validator-part-ii-client-code.aspx)
* [���Ѫ�-Creating Custom Validation Attribute in ASP.NET MVC](http://blog.andrei.rinea.ro/2013/06/28/building-client-javascript-custom-validation-in-asp-net-mvc-4-using-jquery/)
* [Handling Multiple Validation Rules in ASP.NET MVC Using DataAnnotations and Unobtrusive jQuery Validator - Part 1](https://www.captechconsulting.com/blogs/handling-multiple-validation-rules-in-aspnet-mvc-using-dataannotations-and-unobtrusive-jquery-validator---part-1)
* [Handling Multiple Validation Rules in ASP.NET MVC Using DataAnnotations and Unobtrusive jQuery Validator - Part 2](https://www.captechconsulting.com/blogs/handling-multiple-validation-rules-in-aspnet-mvc-using-dataannotations-and-unobtrusive-jquery-validator---part-2)