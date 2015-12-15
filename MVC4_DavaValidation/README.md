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
# ���Ϊ��ѦҸ��
* [�ڰѦҳo�g�إߪ�l�d��- ASP.NET MVC Model Validation using Data Annotations](https://joeylicc.wordpress.com/2013/06/20/asp-net-mvc-model-validation-using-data-annotations/)