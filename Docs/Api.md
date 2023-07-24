# Buber breakfast API 
Hello

```
dotnet new sln -o BuberBreakfast       
dotnet new classlib -o BuberBreakfast.Contracts    
dotnet new webapi -o BuberBreakfast   
dotnet build  
dotnet sln add ./BuberBreakfast.Contracts ./BuberBreakfast/  
dotnet add BuberBreakfast/ reference BuberBreakfast.Contracts
dotnet run --project BuberBreakfast
dotnet add BuberBreakfast package ErrorOr
```