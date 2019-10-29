# Waes Diff Service

Waes Diff Service is an API to compare binary data encoded in base64.

## Building and Running

Clone this repo and run `dotnet restore` to install all the dependencies.

Once the dependencies are installed you can run it with `dotnet run` . You will then be able to run requests agains localhost:5001

## Usage

Endpoints

* {host}/v1/diff/{id}/left	 	(POST) Set the left side to diff
* {host}/v1/diff/{id}/right		(POST) Set the left side to diff
* {host}/v1/diff/{id}			(GET) Gets the diff result

Data Types

* Id should be a Guid
* JSON object should contain the data to be diff-ed in encoded in Base64, es following example

```
{
    "data": "V2UgYXJlIGFsbCBzdG9yaWVzIGluIHRoZSBlbmQ="
}
```

Response

* POST endpoints will return 200 Http Status Code on success

* GET will return an object infoming the diff status (Equal, Not Equal, Unmatched Size). If data is not equal in both sides the response will include the detail of each difference (Offset and Length)

## Sample requests and response

* Requests
```
curl -X POST "{Host}/v1/diff/12aaabc5-a0b0-4fe7-a5a2-ed8d65196bfe/left" -H "accept: */*" -H "Content-Type: application/json" -d "{\"data\":\"V2UgYXJlIGFsbCBzdG9yaWVzIGluIHRoZSBlbmQ=\"}"
```

```
curl -X POST "{Host}/v1/diff/12aaabc5-a0b0-4fe7-a5a2-ed8d65196bfe/right" -H "accept: */*" -H "Content-Type: application/json" -d "{\"data\":\"V2UgYXJlIGFsbCBzdG9yaWVzIGluIHRoZSBlbmQ=\"}"
```

```
curl -X GET "{Host}/v1/diff/12aaabc5-a0b0-4fe7-a5a2-ed8d65196bfe" -H "accept: application/json"
```

* Response
```
{
  "status": "Not Equal",
  "differences": [
    {
      "offset": 1,
      "length": 3
    }
  ]
}
```
