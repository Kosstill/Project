# Project

## API

**Get**

Request: `/api/organization`

Body:

**Get item**

Request: `/api/organization/1`

Body:

**Create**

Request: `/api/organization`

Body:

```
{
  "name": "Organization 1",
  "code": 201,
  "organizationType": "General partnership",
  "owner": "Person 1"
}
```

**Update**

Request: `/api/organization/1`

Body:
```
{
  "Id": 1,
  "name": "Organization 1",
  "code": 201,
  "organizationType": "General partnership",
  "owner": "Person 1"
}
```

**Delete**

Request: `/api/organization/1`