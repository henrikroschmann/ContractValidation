# Contract Validation and Ingestion API

This solution demonstrates how to implement a custom validation service in an ASP.NET Core API to handle complex validation scenarios, including custom attributes and processing valid and invalid records separately.

## Table of Contents

-   [Overview](#overview)
-   [Setup](#setup)
-   [Usage](#usage)
-   [Examples](#examples)

## Overview

This API allows for the ingestion of data with validation rules defined using attributes on the input contracts. The solution supports standard validation attributes (e.g., `Required`, `MinLength`, `Range`) as well as custom attributes (e.g., `NotDefaultGuid`). Valid and invalid records are processed separately, with appropriate responses returned to the client.

## Setup

1.  **Disable Default Model State Validation**: To ensure that our custom validation logic is used, we need to disable the default model state validation.
 ```csharp
 builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
```

2. **Define Validation Attributes**: Add validation attributes to your input contracts.

```csharp
public class PutProductsRequest
{
    [Required]
    public string Source { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = $"{nameof(Name)} should not be empty")]
    public string Name { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Price cannot be negative")]
    public int Price { get; set; }
}
```

## Usage

### Controller

Use the validation service in your controller to validate input data and handle success and failure scenarios.

```csharp
[ApiController]
[Route("/api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IContractsValidationService _validationService;
    private readonly IMediator _mediator;

    public ProductController(IContractsValidationService validationService, IMediator mediator)
    {
        _validationService = validationService;
        _mediator = mediator;
    }

    [HttpPost]
    public IActionResult AddProducts(PutProductsRequest products)
    {
        var validationResult = _validationService.Validate(products);
        var (Success, Data) = validationResult
                .OnSuccess(validRecords => _mediator.Send(new ProductIngestionsRequest(validRecords)))
                .OnFailure((invalidValue, errors) => _mediator.Send(new RejectedProductsRequest(invalidValue, errors)))
                .HandleResponse();

        return Success
            ? Ok(Data)
            : BadRequest(Data);
    }
}
```
## Examples

### Valid Input

**Request:**
```csharp
POST https://localhost:5001/api/Product
Content-Type: application/json
Accept-Language: en-US,en;q=0.5

{
    "source": "SomeSource",
    "name": "ProductName",
    "price": 10,
    "someGuid": "84b242ba-0e5d-4c6d-b220-3e4c38de872c",
    "data": [
        {
            "name": "Item1",
            "price": 4
        },
        {
            "name": "Item2",
            "price": 7
        }
    ]
}
```
Response:
```csharp
{
    "source": "SomeSource",
    "name": "ProductName",
    "price": 10,
    "someGuid": "84b242ba-0e5d-4c6d-b220-3e4c38de872c",
    "data": [
        {
            "name": "Item1",
            "price": 4
        },
        {
            "name": "Item2",
            "price": 7
        }
    ]
}
```

### Invalid Input

**Request:**
```csharp
POST https://localhost:5001/api/Product
Content-Type: application/json
Accept-Language: en-US,en;q=0.5

{
    "source": "SomeSource",
    "name": "",
    "price": -1,
    "someGuid": "84b242ba-0e5d-4c6d-b220-3e4c38de872c",
    "data": [
        {
            "name": "",
            "price": 6
        }
    ]
}
```

Response:
```csharp
{
    "invalidValue": {
        "source": "SomeSource",
        "name": "",
        "price": -1,
        "someGuid": "84b242ba-0e5d-4c6d-b220-3e4c38de872c",
        "data": [
            {
                "name": "",
                "price": 6
            }
        ]
    },
    "errors": [
        "The Name field is required.",
        "Price cannot be negative"
    ]
}
```
