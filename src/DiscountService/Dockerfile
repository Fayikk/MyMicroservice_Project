FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80


COPY MyMicroservice_Project.sln MyMicroservice_Project.sln
COPY src/GameService/GameService.csproj src/GameService/GameService.csproj 
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj 
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj 
COPY src/FilterService/FilterService.csproj src/FilterService/FilterService.csproj 
COPY src/OrderService/OrderService.csproj src/OrderService/OrderService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj 
COPY src/DiscountService/DiscountService.csproj src/DiscountService/DiscountService.csproj 
COPY src/BasketService/BasketService.csproj src/BasketService/BasketService.csproj 



RUN dotnet restore MyMicroservice_Project.sln



COPY src/DiscountService src/DiscountService
COPY src/Contracts src/Contracts
WORKDIR /app/src/DiscountService 
RUN dotnet publish -c Release -o /app/src/out


FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet" ,"DiscountService.dll" ]