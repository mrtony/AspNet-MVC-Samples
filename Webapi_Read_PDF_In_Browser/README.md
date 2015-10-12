# Sample - 以WEBAPI傳回PDF讓Browser直接開啟

### 修改內容
* routing
* 於ValuesController中新增action以在browser直接顯示pdf

### 重點
* Media type header的MIME要設對
```
result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/pdf");
``` 

### 參考資源
http://forums.asp.net/t/1824120.aspx?returning+a+pdf+from+a+Web+Api+get
http://stackoverflow.com/questions/9541351/returning-binary-file-from-controller-in-asp-net-web-api
