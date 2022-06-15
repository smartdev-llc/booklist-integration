
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TLM.Books.API/TLM.Books.API.csproj", "TLM.Books.API/"]
COPY ["TLM.Books.Infrastructure/TLM.Books.Infrastructure.csproj", "TLM.Books.Infrastructure/"]
COPY ["TLM.Books.Application/TLM.Books.Application.csproj", "TLM.Books.Application/"]
COPY ["TLM.Books.Domain/TLM.Books.Domain.csproj", "TLM.Books.Domain/"]
COPY ["TLM.Books.Common/TLM.Books.Common.csproj", "TLM.Books.Common/"]

COPY ./Setup.sh ./Setup.sh
RUN dotnet tool install --global dotnet-ef

RUN dotnet restore "TLM.Books.API/TLM.Books.API.csproj"
COPY . .
WORKDIR "/src/."

RUN /root/.dotnet/tools/dotnet-ef migrations add InitialMigrations -s TLM.Books.API/TLM.Books.API.csproj -p TLM.Books.Infrastructure/TLM.Books.Infrastructure.csproj

RUN chmod +x ./Setup.sh
CMD /bin/bash ./Setup.sh