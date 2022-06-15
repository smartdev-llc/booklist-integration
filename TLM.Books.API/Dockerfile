#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TLM.Books.API/TLM.Books.API.csproj", "TLM.Books.API/"]
COPY ["TLM.Books.Infrastructure/TLM.Books.Infrastructure.csproj", "TLM.Books.Infrastructure/"]
COPY ["TLM.Books.Application/TLM.Books.Application.csproj", "TLM.Books.Application/"]
COPY ["TLM.Books.Domain/TLM.Books.Domain.csproj", "TLM.Books.Domain/"]
COPY ["TLM.Books.Common/TLM.Books.Common.csproj", "TLM.Books.Common/"]
RUN dotnet restore "TLM.Books.API/TLM.Books.API.csproj"
COPY . .
WORKDIR "/src/TLM.Books.API"
RUN dotnet build "TLM.Books.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TLM.Books.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TLM.Books.API.dll"]