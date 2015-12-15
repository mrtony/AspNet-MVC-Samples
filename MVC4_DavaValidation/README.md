MVC4 表單驗證
===

# 使用DataAnnotation提供的內建驗證
MVC的DataAnnotation提供了內建的驗證, 只要引入後使用DataType定義好, 就會自動做前後端的驗證.

### 使用DataType=Email的範例
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

### 使用ModelState.AddError
MVC也提供了在POST資料後, 在後端可以加入自己的商業邏輯判斷後, 並自訂錯誤訊息顯示. 而且一旦執行了AddError, `ModelState.IsValid`就一定會是false.

```
//Model
public string AddErrorEmail { get; set; }
//Controller
public ActionResult Index(User model)
{
    if (model.AddErrorEmail.Equals("t@gmail.com"))
    {
        ModelState.AddModelError("ModelStateError", "Email不可以是t@gmail.com");
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
# 有用的參考資料
* [我參考這篇建立初始範例- ASP.NET MVC Model Validation using Data Annotations](https://joeylicc.wordpress.com/2013/06/20/asp-net-mvc-model-validation-using-data-annotations/)