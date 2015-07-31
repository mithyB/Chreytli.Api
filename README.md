# Chreytli.Api Documentation

## Authorization
Some of the following requests require authorization. In order to be authorized a user must first login. 
The response data of the login request contains an `access_token` which must be stored on the client side.
To logout simply delete the `access_token`.

Requests which require authorization are marked with a `*` below. 
For these requets send the `Authorization` header as following:
```
Authorization: Bearer <access_token | string>
```

## Account
### Login
URL
```
POST /token
```
Body
``` 
{
  "grant_type": "password",
  "userName": <username | string>,
  "password": <password | string>
}
```
### Register
URL
```
POST /api/Account/Register
```
Body
``` 
{
  "Username": <username | string>,
  "Email": <email | string>,
  "Password": <password | string>,
  "ConfirmPassword": <confirm password | string>
}
```
## Events
### GET
Returns a list of events.
URL
```
GET /api/Events
```
### POST*
Add a new event. Returns the newly created event.
URL
```
POST /api/Events
```
Body
```
{
  "title": <title | string>,
  "description": <description | string>,
  "start": <start | DateTime>,
  "end": <end | DateTime>,
  "allDay": <allDay | bool>
}
```
### PUT*
Edit a existing event. Returns the edited event.
URL
```
PUT /api/Events
```
Body
```
{
  "id": <id | string>,
  "title": <title | string>,
  "description": <description | string>,
  "start": <start | DateTime>,
  "end": <end | DateTime>,
  "allDay": <allDay | bool>
}
```
### DELETE*
Delete an event by its ID. Returns the deleted event.
URL
```
DELETE /api/Events
```
Body
```
{
  "id": <id | string>
}
```
