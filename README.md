# Project

The test project

## API

The description how to test API

### Organization

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

### Country

**Get**

Request: `/api/country`

Body:

**Get item**

Request: `/api/country/1`

Body:

**Create**

Request: `/api/country`

Body:

```
{
  "name": "Country 1",
  "code": 201,
  "OrganizationId": 1
}
```

**Update**

Request: `/api/country/1`

Body:
```
{
  "Id": 1,
  "name": "Organization 1",
  "code": 201,
  "OrganizationId": 1
}
```

**Delete**

Request: `/api/country/1`

### Business

**Get**

Request: `/api/business`

Body:

**Get item**

Request: `/api/business/1`

Body:

**Create**

Request: `/api/business`

Body:

```
{
  "name": "Business 1",
  "CountryId": 1
}
```

**Update**

Request: `/api/business/1`

Body:
```
{
  "Id": 1,
  "name": "Business 1",
  "CountryId": 1
}
```

**Delete**

Request: `/api/business/1`

### Family

**Get**

Request: `/api/family`

Body:

**Get item**

Request: `/api/family/1`

Body:

**Create**

Request: `/api/family`

Body:

```
{
  "name": "Family 1",
  "BusinessId": 1
}
```

**Update**

Request: `/api/family/1`

Body:
```
{
  "Id": 1,
  "name": "Family 1",
  "BusinessId": 1
}
```

**Delete**

Request: `/api/family/1`

### Offering

**Get**

Request: `/api/offering`

Body:

**Get item**

Request: `/api/offering/1`

Body:

**Create**

Request: `/api/offering`

Body:

```
{
  "name": "Offering 1",
  "FamilyId": 1
}
```

**Update**

Request: `/api/offering/1`

Body:
```
{
  "Id": 1,
  "name": "Offering 1",
  "FamilyId": 1
}
```

**Delete**

Request: `/api/offering/1`

### Department

**Get**

Request: `/api/department`

Body:

**Get item**

Request: `/api/department/1`

Body:

**Create**

Request: `/api/department`

Body:

```
{
  "name": "Department 1",
  "OfferingId": 1
}
```

**Update**

Request: `/api/department/1`

Body:
```
{
  "Id": 1,
  "name": "Department 1",
  "OfferingId": 1
}
```

**Delete**

Request: `/api/department/1`
