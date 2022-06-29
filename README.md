# 示例

```
HttpResult<string> httpResult = httpClient.SetBaseUri("基础地址").AppendSegments("路由片段")
                .WithQueryParams(new { key1 = "value1", key2 = "value2" })
                .WithHeaders(new Dictionary<string, object>() { { "Connection", "keep-alive" } })
                .WithContent("JSON文本字符串")
                .GetStringAsync().Result;
```
