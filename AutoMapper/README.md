AutoMapper
===

AutoMapper是在.net中可以執行自動映射的套件, 只要簡單的建立來源/目的類別, 就會依照名稱去自動對應將來源的值複製到目的實體中.
它也可以將深層的物件做平行化.


# Automapper的Helloworld
1. 安裝Automapper: `Install-Package AutoMapper`
2. 建立映射並執行映射

```
var customer = new Customer() {FirstName = "Tony", LastName = "Chen", Address = "Taipei", Age = 30};
var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerView>());
var mapper = config.CreateMapper();

var destination = mapper.Map<CustomerView>(customer);
Console.WriteLine("CustomerView內容:{0}", destination.Dump());
```

# Custom value resolver - 自定映射的一種作法
要將來源物件的FirstName+LastName後, 映射到目的物件的Name要怎麼做? 可以用`Custom value resolver`.

Automapper提供`ValueResolver`抽像類別處理這個問題, 只要繼承並實作就可以達成.

```
public class CustomerView : ValueResolver<Customer, string>
{
    public string Name { get; set; }
    public string Address { get; set; }

    protected override string ResolveCore(Customer source)
    {
        return source.FirstName + "-" + source.LastName;
    }
}

var customer = new Customer() { FirstName = "Tony", LastName = "Chen", Address = "Taipei", Age = 30 };
var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerView>()
    .ForMember(dest => dest.Name, opt => opt.ResolveUsing<CustomerView>()));
var mapper = config.CreateMapper();

var destination = mapper.Map<CustomerView>(customer);
```

# Domain
```
public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }


    public string GetFullname()
    {
        return this.FirstName + "-" + this.LastName;
    }
}

public class CustomerClone
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}

public class CustomerView : ValueResolver<Customer, string>
{
    public string Name { get; set; }
    public string Address { get; set; }

    protected override string ResolveCore(Customer source)
    {
        return source.FirstName + "-" + source.LastName;
    }
}
```
