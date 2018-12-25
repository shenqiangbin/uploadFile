# 实现基本的文件上传

## 知识点


实现上传还是使用的type=file的input控件，在表单中注意加【enctype=multipart/form-data】属性

在后台使用类型为【HttpPostedFileBase】的参数进行接收文件即可，然后使用SaveAs方法进行保存即可

## 可能碰到的错误
错误：HttpException (0x80004005): 超过了最大请求长度

在配置文件中添加 `<httpRuntime targetFramework="4.5" maxRequestLength="10240" />`

错误：404 错误，文件太大导致的文件上传失败

```
<security>
<requestFiltering>
<requestLimits maxAllowedContentLength="524288000" /><!-- 5GB-->
</requestFiltering>
</security>
```

## 添加Uploadify的上传功能
