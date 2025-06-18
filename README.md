### The Milvus Vector Database RESTful API

This is a .NET Core based RESTful API for testing the functionality of Milvus Vector Database.

To get the inital knowledge and config the DB, follow the Medium Article Series on <a href="https://medium.com/@malindumadhubashana/a-beginners-guide-to-milvus-vector-database-part-i-2e84a11a29d2">Here</a>

### Example Request 

```/api/Vector/annsearch```

```
{
  "queryVector": [0.35, -0.60, 0.18, 0.22],
  "topK": 6,
  "collectionName": "iris_data",
  "fieldName": "features"
}

```
