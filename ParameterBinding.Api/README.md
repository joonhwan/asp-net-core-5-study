# asp-net-core-5-study


# AspNetCore Web API 기초 - 파라메터 바인딩

Created By: Joonhwan Lee
Last Edited: June 28, 2021 10:28 AM
Property 1: No

# 파라메터(매개변수) 바인딩이란.

Web API 에서 파라메터 바인딩이란,  클라이언트측에서 송신한 다음과 같은 형식의 HTTP Request Message의 다음 각 부분

- URL(Request Line)
- Request Header
- Request Body

에 들어있는 특정 데이터가,  Controller 클래스의 특정 Method(= "**Action Method**" 라고 함)의 파라메터에 어떻게 연계(바인딩)되는 지와 관련한 내용입니다.

HTTP Request Message는 아래처럼 구성됩니다.

![https://documentation.help/DogeTool-HTTP-Requests-vt/http_requestmessageexample.png](https://documentation.help/DogeTool-HTTP-Requests-vt/http_requestmessageexample.png)

상기 요청에 대한 Controller의 Action Method는 아래처럼 구성될 수 있습니다.  

```csharp
[ApiController]
[Route("/doc/test.html")]
public class **BookController** : ControllerBase
{
	[HttpGet]
	public Book Get([FromForm]BookSearch bookSearch)
	{
	   // ...
	} 
}

public class BookSearch
{
   public string **BookId** { get; set; }
   public string **Author** { get; set; }
}
```

( 주의: 개념적인 코드임. 동작 보장하지 않음)

상기 Action Method에 `bookSearch` 객체는 `BookId`  에는 `12345` 가, `Author` 에는 `Tan+Ah+Teck` 가 들어있는 상태로 Asp Net Core 프레임웍에 의해 변환되어 호출됩니다. 

## 사전지식 1: URL 구성 요소

[https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2FCpfdy%2FbtqTIpUqjys%2Ft9CnMI2krtz3Sla4kUU70K%2Fimg.png](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2FCpfdy%2FbtqTIpUqjys%2Ft9CnMI2krtz3Sla4kUU70K%2Fimg.png)

[https://ulralra-dev.tistory.com/20](https://ulralra-dev.tistory.com/20) 참고

# HTTP Method에 따르는 파라메터 바인딩 기본 처리 방식

ASP NET Core에서 Controller 클래스에 `[ApiController]` 어트리뷰트가 있는 경우에는 다음과 같이 , API 를 위한 기본 파라메터 바인딩 처리 방식이 적용됩니다.

Action Method 의 파라메터에는 통상의 C# 함수 처럼,  

- 기본 타입 : int, bool, double, string, GUID, DateTime, decimal 또는 문자열
- 복합 타입 : BookSearch,  LotData, TrainingJob, ... 같은 사용자 정의 타입

를 사용할 수 있습니다. 

ASP NET CORE의 Web API 는 기본적으로 

- Query String(쿼리문자열)  : URL 경로 뒤 `?` 문자 뒤에 오는 Key Value Pair 문자열
- HTTP Request Body : JSON(기본값), XML, .....

로 부터 파라메터 바인딩을 HTTP METHOD 에 따라 아래 처럼 수행해 줍니다. 

[Untitled](https://www.notion.so/ff7e559f5cf242a78b6ba1e25d291fd5)

## Query String 으로 부터 기본 타입 Binding  : GET, POST, PUT, PATCH, DELETE

```csharp
[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
    [HttpGet]
    public Pet Get(int id) 
    {   
    }
}
```

위에서 볼 수 있듯이 Get action 메소드는 int 유형의 id 매개 변수를 포함합니다. 따라서 Web API는 요청 된URL의 쿼리 문자열에서 id 값을 추출하여 int로 변환하여 Get action 메서드의 id 매개 변수에 할당하려고합니다. 예를 들어 HTTP 요청이 [`h](http://localhost/api/student?id=1%EC%9D%B4%EB%A9%B4)ttp://localhost/api/pets?id=1` 이면,  id 매개 변수의 값은 1이됩니다.

다음은 위의 작업 방법에 대한 유효한 HTTP GET 요청입니다.

`http://localhost/api/pets?id=1`

`http://localhost/api/pets?ID=1`

> 쿼리 문자열 매개 변수 이름과 조치 메소드 매개 변수 이름은 동일해야합니다 (대소 문자 구분 안 함). 이름이 일치하지 않으면 매개 변수 값이 설정되지 않습니다. 매개 변수의 순서는 다를 수 있습니다.

## Query String 으로 부터 기본 타입 Binding  #2  : GET, POST, PUT, PATCH, DELETE

```csharp
[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
    [HttpGet]
    public Pet Get(int id, string name) 
    {   
    }
}
```

위에서 볼 수 있듯이 Get 메서드에는 여러 기본 형식 매개 변수가 포함되어 있습니다. 따라서 Web API는 요청 URL의 쿼리 문자열에서 값을 추출하려고 시도합니다. 예를 들어 HTTP 요청이 있으면 `http://localhost/api/pets?id=1&name=steve` 매개 변수의 값은 1이되고 이름은 "merry"가됩니다.

다음은 위의 작업 방법에 대한 유효한 HTTP GET 요청입니다.

`http://localhost/api/pets?id=1&name=steve`

`http://localhost/api/pets?ID=1&NAME=steve`

`http://localhost/api/pets?name=steve&id=1`

> 쿼리 문자열 매개 변수 이름은 작업 메서드 매개 변수의 이름과 일치해야합니다. 그러나 순서가 다를 수 있습니다.

## Query String 으로 부터 복합 타입 Binding : GET, DELETE

```csharp
[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
    [HttpGet]
    public Pet Find(Pet pet) 
    {   
    }
}

public class Pet 
{ 
   public int Id { get; set; }
   public int Age { get; set; } 
   public string Name { get; set; }
}
```

GET이나 DELETE Action Method의 경우, 파라메터 인자에 복합 타입을 사용하는 경우, 복합 타입의 클래스 속성 명칭 각각에 대한 인자가 전달되는 것과 유사하게 처리할 수 있습니다. 

즉, `[http://localhost/api/pets?age=1&name=steve](http://localhost/api/pets?age=1&name=steve)` 의 요청에 대하여 상기 Action Method의 `pet` 속성은 `Id` 속성을 제외한 나머지 값들이 옳바르게 바인딩된 상태로 전달됩니다. (`Id` 의 경우 초기화 되지 않습니다)

> Query String 을 복합 타입으로 바인딩 할 경우, 복합 타입의 모든 속성이 기본 타입이어야 합니다

## Request Body 로 부터 복합 타입 Binding : POST, PUT, DELETE

```csharp
[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
    [HttpPost]
    public Pet Add(Pet pet) 
    {   
    }
}
```

위 Action Method는 다음과 같은 HTTP Request Message 에 의해 파라메터의 값이 바인딩됩니다 

```csharp
POST https//localhost:5001/api/pets
Content-Type: application/json
Content-Length: 13
{ "age":1, "name":"steve" }

```

> 바인딩시 원본에 존재하지 않는 데이터, 즉, 위 경우 `Id` 속성은 `default(int)` 즉, `0` 값이 들어갑니다

## Query String / Request Body 로 부터 기본/복합 타입 동시  Binding : POST, PUT, DELETE

```csharp
[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
    [HttpPut]
    public Pet Update(int id, Pet pet) 
    {   
    }
}

```

위의 경우, 기본 타입인 `id` 파라마터는 Query String으로 부터 가져오고, 복합 타입인 `pet` 파라메터는 Request Body 로 부터 바인딩됩니다.   아래의 HTTP Request Message 가 수신되면

```csharp
PUT https//localhost:5001/api/pets?id=10
Content-Type: application/json
Content-Length: 13
{ "age":2, "name":"steve" }
```

상기 Action Method의 `id` 에는 `10` , `pet`  객체의 `Age` 와 `Name` 속성에는 각각 `2` 와 `steve` 가 들어있는 상태로 전달됩니다.

# 기본 파라메터 바인딩 특성을 변경하는 법.

위에서 기술된 내용 이외의 바인딩을 하려면(예를들어, GET Action Method의  복합 타입 파라메터를 Request Message Body에서 가져오고 싶다....), 통상 Action Method의 각 인자앞에 아래와 같은 Attribute를 지정하여 해당 값이 원하는 Message의 부분에서 값이 바인딩되도록 할 수 있다. 

- `[FromBody]` 어트리뷰트 : Body 에서 해당 파라메터를 가져온다
- `[FromQuery]` 어트리뷰트 : 쿼리 문자열에서 해당 파라메터를 가져온다
- `[FromRoute]` 어트리뷰트 : URL의 경로 문자열에서 파라메터를 가져온다
- `[FromForm]` 어트리뷰트 :  HTTP Form 데이터에서 파라메터를 가져온다. **파일개체를 전달할 수 있다(파일첨부기능).**
- `[FromHeader]` 어트리뷰트 : HTTP Request의 Header 정보에 있는 key-value pair 로 부터 파라메터를 가져온다
- `[FromService]` 어트리뷰트 : 값 전달과 상관 없지만, 특정 인자를 외부로 부터 의존성 주입하도록 할 수 있다.

> 값에 `%2f`(즉, `/`)이 포함될 수 있는 경우 `[FromRoute]`를 사용하지 않습니다. `%2f`는 `/`로 이스케이프가 해제되지 않습니다. 값에 `%2f` 가 포함될 수 있으면 `[FromQuery]`를 사용합니다.

공식문서 참조

[https://docs.microsoft.com/ko-kr/aspnet/core/web-api/?view=aspnetcore-5.0#binding-source-parameter-inference](https://docs.microsoft.com/ko-kr/aspnet/core/web-api/?view=aspnetcore-5.0#binding-source-parameter-inference) 

# `ApiController` 어트리뷰트와 파라메터 바인딩

Web API 구성시 종종 Controller 클래스 앞에 붙이는 어트리뷰트 `[ApiController]` 는 WEB API의 구성시 파라메터 바인딩과 관련하여 작업의 일관성과 편리함을 주기위해 아래와 같이 기본 파라메터 바인딩을 유추합니다. 이러한 규칙을 통해 작업 매개 변수에 특성을 적용하여 바인딩 소스를 수동으로 식별할 필요가 없습니다(맨 위에서 설명한 대로...). 바인딩 소스 유추 규칙은 다음과 같이 동작합니다.

- `[FromBody]`는 복합 타입 매개 변수에 대해 유추됩니다. `[FromBody]` 유추 규칙은 [IFormCollection](https://docs.microsoft.com/ko-kr/dotnet/api/microsoft.aspnetcore.http.iformcollection) 및 [CancellationToken](https://docs.microsoft.com/ko-kr/dotnet/api/system.threading.cancellationtoken) 같이 특별한 의미를 지닌 복합 기본 타입 형식에는 적용되지 않습니다. 바인딩 소스 유추 코드는 이러한 특별한 형식을 무시합니다.
- `[FromForm]`은 [IFormFile](https://docs.microsoft.com/ko-kr/dotnet/api/microsoft.aspnetcore.http.iformfile) 및 [IFormFileCollection](https://docs.microsoft.com/ko-kr/dotnet/api/microsoft.aspnetcore.http.iformfilecollection) 타입의 작업 매개 변수에 대해 유추됩니다. 단순 또는 사용자 정의 타입에 대해서는 유추되지 않습니다.
- `[FromRoute]`는 경로 템플릿에서 매개 변수와 일치하는 작업 매개 변수 이름에 대해 유추됩니다. 둘 이상의 경로가 작업 매개 변수와 일치하는 경우 모든 경로 값은 `[FromRoute]`로 간주됩니다.
("경로 템플릿"이란 `[HttpGet("/{id}/{name}")]` 과 같이 Action Method에 어트리뷰트 설정할 경우 `"/{id}/{name}"` 부분을 의미합니다)
- `[FromQuery]`는 다른 작업 매개 변수에 대해 유추됩니다.

과 같습니다.
