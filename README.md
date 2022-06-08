# fido2me

## Local Deployment
You need to create a special secret file in the "<USER>\AppData\Roaming\Microsoft\UserSecrets\19d06836-b10d-4105-b995-a6057bbe2d2d" folder with the following content.

```
  "cosmosdb-privatekey": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
  "cosmosdb-endpointurl": "https://localhost:8081/",
  "cosmosdb-dbname": "fidolocal",
  "applicationinsights-instrumentationkey": "<Your Application Insights ID"
```

You will need to install the Azure Cosmos DB emulator or use a cloud database instance along with created an Application Insight service instance.