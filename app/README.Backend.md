# API App Guide

## Prerequisites
- Basic knowledge of web app concepts
- C# .NET experience at a beginner level
- Local installations of the .NET Core SDK and Visual Studio 2022
- Read first the infrasture setup posted on the main [README.md](../README.md)

## Azure Cloud Prerequisites
- Azure Function App
- Azure Storage Account
- Azure SQL Database

## Instructions
- After completing the Azure Cloud Prerequisites
- Open RingletBox.Backend.sln on Visual Studio
- Look for localsettings.json, and update the values of following properties
	- ConnectionStrings::DataDbContext - ConnectionStrings of the Azure SQL Database **(Required)**
	- AzureStorageAccount - Azure Storage Account Access Key **(Required)**
	- AzureWebJobsStorage - Azure Storage Account Access Key **(Required)**
	- CirrusNodeApiEndpoint - If you have Cirrus Core Node Instance, update this property. *(Optional)*
	- PartitionKey - Any value, used this to segregate Networks while using single database. *(Optional)*
	- LockContractAddress - If you have Cirrus Core Node Instance, run the function app locally and call **HTTP POST** Request on ***api/smartContract/create*** endpoint, with the following post body
		- ```json
			{
			    "Sender":"WalletAddressSender",
			    "ContractCode":"ByteCodeFoundOnLockTokenVaultREADME.md",
			    "Parameters": []
			}
			```

- To create or update the database, execute the command inside of ***app*** directory, **dotnet ef database update -p .\Infrastructure\Infrastructure.csproj -s .\WebApi\WebApi.csproj**
- Set **WebApi** Project as Startup Project
- Click Run or Press F5.
- If you wish to publish the API to Azure Function App, please see [Guide](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio#publish-the-project-to-azure)